extern crate cgmath as cgm;
extern crate gl;
extern crate glfw;

mod base;
mod graphic;
mod light;
mod primitive;
mod component;

use base::canvas::Canvas;
use base::input::Input;
use base::keyboard::Key;
use base::tick::Tick;

use graphic::api;

use graphic::shader::Shader;
use graphic::shader::ShaderError;
use graphic::shader::ShaderType;

use graphic::program::Program;
use graphic::program::ProgramError;

use graphic::texture::Texture;
use graphic::texture::TextureError;
use graphic::cube_map::CubeMap;
use graphic::cube_map::CubeMapError;

use graphic::camera::Camera;

use primitive::cube::Cube;
use primitive::plane::Plane;
use primitive::sphere::Sphere;
use primitive::sky_box::SkyBox;

use light::Light;

const TITLE: &str = "OpenGL";
const WIDTH: u32 = 1680;
const HEIGHT: u32 = 1080;
const LIGHT_SPEED: f32 = 5.0;
const ENTITY_ROTATION_SPEED: f32 = 200.0;
const CAMERA_ROTATION_SPEED: f32 = 20.0;


pub fn main() {
    let mut canvas = match Canvas::new(TITLE, WIDTH, HEIGHT) {
        Ok(canvas) => canvas,
        Err(_) => {
            println!("Canvas failed");
            return;
        }
    };
    let mut input = Input::new();
    let mut tick = Tick::new();

    let pbr_vertex_shader =
        match Shader::new("assets/shaders/pbr.vertex.glsl", ShaderType::VertexShader) {
            Ok(v) => v,
            Err(ShaderError::FailedOpeningFile) => {
                println!("Failed opening PBR vertex shader file, file may not exist or the path is wrong");
                return;
            }
            Err(ShaderError::FailedReadingFile) => {
                println!("PBR vertex shader file was not readable, check the file content or permission");
                return;
            }
            Err(ShaderError::FailedCompilingShader(error)) => {
                println!(
                    "Compiling PBR vertex shader failed, check shader content\n{}",
                    error
                );
                return;
            }
        };

    let default_vertex_shader =
        match Shader::new("assets/shaders/default.vertex.glsl", ShaderType::VertexShader) {
            Ok(v) => v,
            Err(ShaderError::FailedOpeningFile) => {
                println!("Failed opening default vertex shader file, file may not exist or the path is wrong");
                return;
            }
            Err(ShaderError::FailedReadingFile) => {
                println!("Default vertex shader file was not readable, check the file content or permission");
                return;
            }
            Err(ShaderError::FailedCompilingShader(error)) => {
                println!(
                    "Compiling default vertex shader failed, check shader content\n{}",
                    error
                );
                return;
            }
        };

    let skybox_vertex_shader =
        match Shader::new("assets/shaders/skybox.vertex.glsl", ShaderType::VertexShader) {
            Ok(v) => v,
            Err(ShaderError::FailedOpeningFile) => {
                println!("Failed opening skybox vertex shader file, file may not exist or the path is wrong");
                return;
            }
            Err(ShaderError::FailedReadingFile) => {
                println!("Skybox vertex shader file was not readable, check the file content or permission");
                return;
            }
            Err(ShaderError::FailedCompilingShader(error)) => {
                println!(
                    "Compiling skybox vertex shader failed, check shader content\n{}",
                    error
                );
                return;
            }
        };

    let pbr_fragment_shader = match Shader::new(
        "assets/shaders/pbr.fragment.glsl",
        ShaderType::FragmentShader,
    ) {
        Ok(v) => v,
        Err(ShaderError::FailedOpeningFile) => {
            println!("Failed opening PBR fragment shader file, file may not exist or the path is wrong");
            return;
        }
        Err(ShaderError::FailedReadingFile) => {
            println!("PBR fragment shader file was not readable, check the file content or permission");
            return;
        }
        Err(ShaderError::FailedCompilingShader(error)) => {
            println!(
                "Compiling PBR fragment shader failed, check shader content:\n{}",
                error
            );
            return;
        }
    };

    let default_fragment_shader = match Shader::new(
        "assets/shaders/default.fragment.glsl",
        ShaderType::FragmentShader,
    ) {
        Ok(v) => v,
        Err(ShaderError::FailedOpeningFile) => {
            println!("Failed opening default fragment shader file, file may not exist or the path is wrong");
            return;
        }
        Err(ShaderError::FailedReadingFile) => {
            println!("Default fragment shader file was not readable, check the file content or permission");
            return;
        }
        Err(ShaderError::FailedCompilingShader(error)) => {
            println!(
                "Compiling default fragment shader failed, check shader content:\n{}",
                error
            );
            return;
        }
    };

    let skybox_fragment_shader = match Shader::new(
        "assets/shaders/skybox.fragment.glsl",
        ShaderType::FragmentShader,
    ) {
        Ok(v) => v,
        Err(ShaderError::FailedOpeningFile) => {
            println!("Failed opening skybox fragment shader file, file may not exist or the path is wrong");
            return;
        }
        Err(ShaderError::FailedReadingFile) => {
            println!("Skybox fragment shader file was not readable, check the file content or permission");
            return;
        }
        Err(ShaderError::FailedCompilingShader(error)) => {
            println!(
                "Compiling skybox fragment shader failed, check shader content:\n{}",
                error
            );
            return;
        }
    };

    let pbr_program = match Program::new(pbr_vertex_shader, pbr_fragment_shader) {
        Ok(program) => program,
        Err(ProgramError::FailedLinkingShader(error)) => {
            println!("Linking PBR program failed: \n{}", error);
            return;
        }
    };

    let default_program = match Program::new(default_vertex_shader, default_fragment_shader) {
        Ok(program) => program,
        Err(ProgramError::FailedLinkingShader(error)) => {
            println!("Linking default program failed: \n{}", error);
            return;
        }
    };

    let skybox_program = match Program::new(skybox_vertex_shader, skybox_fragment_shader) {
        Ok(program) => program,
        Err(ProgramError::FailedLinkingShader(error)) => {
            println!("Linking skybox program failed: \n{}", error);
            return;
        }
    };

    let mut cube = Cube::new();
    cube.translate(cgm::Vector3::new(-1.5, 1.0, -7.0));

    let mut pbr_cube = Cube::new();
    pbr_cube.translate(cgm::Vector3::new(1.5, 1.0, -7.0));

    let mut sphere = Sphere::new(1.0, 10, 10);
    sphere.translate(cgm::Vector3::new(-1.5, -1.5, -7.0));

    let mut pbr_sphere = Sphere::new(1.0, 10, 10);
    pbr_sphere.translate(cgm::Vector3::new(1.5, -1.5, -7.0));

    let mut plane = Plane::new(2.0, 2.0);
    plane.translate(cgm::Vector3::new(0.0, -4.0, -7.0));

    let skybox = SkyBox::new();

    let mut camera = Camera::perspective(45.0, (WIDTH / HEIGHT) as f32, 0.1, 1000.0);

    let pbr_texture_images = [
        "assets/textures/albedo.png",
        "assets/textures/normal.png",
        "assets/textures/metallic.png",
        "assets/textures/roughness.png",
        "assets/textures/ao.png"
    ];

    let mut pbr_textures = Vec::with_capacity(pbr_texture_images.len());

    for image in pbr_texture_images.iter() {
        let texture = match Texture::new(image) {
            Ok(texture) => texture,
            Err(TextureError::OpeningTextureFailed) => {
                println!("Loading {} pbr texture failed", image);
                return;
            }
        };

        pbr_textures.push(texture);
    }

    let crate_texture = match Texture::new("assets/textures/crate.jpg") {
        Ok(texture) => texture,
        Err(TextureError::OpeningTextureFailed) => {
            println!("Loading crate texture failed");
            return;
        }
    };

    let skybox_map_texture = match CubeMap::new(
        "assets/textures/skybox_right.jpg",
        "assets/textures/skybox_left.jpg",
        "assets/textures/skybox_top.jpg",
        "assets/textures/skybox_bottom.jpg",
        "assets/textures/skybox_back.jpg",
        "assets/textures/skybox_front.jpg"
    ) {
        Ok(cube_map) => cube_map,
        Err(CubeMapError::OpeningTextureFailed(error_message)) => {
            println!("{}", error_message);
            return;
        }
    };


    let mut light = Light::new(
        cgm::Vector3::new(0.0, 1.0, 0.0),
        cgm::Vector3::new(0.75, 0.75, 1.0),
        cgm::Vector3::new(0.75, 0.75, 1.0),
        0.5,
        1.0,
        0.8,
        1.0,
        1.0,
        1.0,
        64.0,
    );

    // settings
    unsafe {
        gl::TexParameteri(
            gl::TEXTURE_2D,
            gl::TEXTURE_WRAP_S,
            gl::MIRRORED_REPEAT as i32,
        );
        gl::TexParameteri(
            gl::TEXTURE_2D,
            gl::TEXTURE_WRAP_T,
            gl::MIRRORED_REPEAT as i32,
        );

        gl::TexParameteri(gl::TEXTURE_2D, gl::TEXTURE_MAG_FILTER, gl::LINEAR as i32);
        gl::TexParameteri(gl::TEXTURE_2D, gl::TEXTURE_MIN_FILTER, gl::LINEAR as i32);
    }

    while !canvas.should_close() {
        canvas.on_update_begin(&mut input);
        tick.on_update();

        api::clear_color(1.0, 0.0, 1.0, 1.0);
        api::clear();

        let mut dir = 0.0;

        // rotation all entities
        if input.is_key_pressed_down(&Key::Q) {
            dir = -1.0;
        } else if input.is_key_pressed_down(&Key::E) {
            dir = 1.0;
        }

        // translation of the camera
        if input.is_key_pressed_down(&Key::W) {
            camera.translate(-cgm::Vector3::unit_y() * tick.delta_time());
        } else if input.is_key_pressed_down(&Key::S) {
            camera.translate(cgm::Vector3::unit_y() * tick.delta_time());
        } else if input.is_key_pressed_down(&Key::D) {
            camera.translate(-cgm::Vector3::unit_x() * tick.delta_time());
        } else if input.is_key_pressed_down(&Key::A) {
            camera.translate(cgm::Vector3::unit_x() * tick.delta_time());
        } else if input.is_key_pressed_down(&Key::R) {
            camera.rotate_x(-1.0 * CAMERA_ROTATION_SPEED * tick.delta_time());
        }  else if input.is_key_pressed_down(&Key::F) {
            camera.rotate_x(1.0 * CAMERA_ROTATION_SPEED * tick.delta_time());
        }

        // move of the light
        if input.is_key_pressed_down(&Key::I) {
            light.add_to_position(cgm::Vector3::unit_y() * LIGHT_SPEED * tick.delta_time());
        } else if input.is_key_pressed_down(&Key::K) {
            light.add_to_position(-cgm::Vector3::unit_y() * LIGHT_SPEED * tick.delta_time());
        } else if input.is_key_pressed_down(&Key::L) {
            light.add_to_position(cgm::Vector3::unit_x() * LIGHT_SPEED * tick.delta_time());
        } else if input.is_key_pressed_down(&Key::J) {
            light.add_to_position(-cgm::Vector3::unit_x() * LIGHT_SPEED * tick.delta_time());
        }

        if input.is_key_pressed_down(&Key::Enter) {
            light.set_light_color(cgm::Vector3::new(tick.time().sin(), tick.time().cos(), tick.time().tan()));
            light.set_ambient_light_color(cgm::Vector3::new(tick.time().sin(), tick.time().cos(), tick.time().tan()));
        }
        // skybox
        api::disable_depth_test();

        skybox_program.bind();
        skybox_map_texture.bind();

        skybox_program.set_mat4f("projection", camera.get_projection());
        skybox_program.set_mat4f("view", camera.get_view());

        skybox.draw();

        // entities
        api::enable_depth_test();

        // PBR
        pbr_program.bind();

        pbr_program.set_int("uAlbedoMap", 0);
        pbr_program.set_int("uNormalMap", 1);
        pbr_program.set_int("uMetallicMap", 2);
        pbr_program.set_int("uRoughnessMap", 3);
        pbr_program.set_int("uAOMap", 4);

        for (index, pbr_texture) in pbr_textures.iter().enumerate() {
            pbr_texture.bind_at_position(index as u32);
        }

        pbr_program.set_mat4f("projection", camera.get_projection());

        pbr_program.set_mat4f("view", camera.get_view());
        pbr_program.set_vec3f("uCameraPos", camera.position());
        pbr_program.set_mat4f("uLight", &light.as_matrix());

        // Cube
        pbr_cube.rotate_y(dir * ENTITY_ROTATION_SPEED * tick.delta_time());
        pbr_cube.draw(&pbr_program);

        // Sphere
        pbr_sphere.rotate_y(dir * ENTITY_ROTATION_SPEED * tick.delta_time());
        pbr_sphere.draw(&pbr_program);

        // Default
        default_program.bind();
        crate_texture.bind_at_position(0);

        default_program.set_mat4f("projection", camera.get_projection());

        default_program.set_mat4f("view", camera.get_view());
        default_program.set_vec3f("uCameraPos", camera.position());
        default_program.set_mat4f("uLight", &light.as_matrix());

        // Cube
        cube.rotate_y(dir * ENTITY_ROTATION_SPEED * tick.delta_time());
        cube.draw(&default_program);

        // Sphere
        sphere.rotate_y(dir * ENTITY_ROTATION_SPEED * tick.delta_time());
        sphere.draw(&default_program);

        // Plane
        plane.rotate_y(dir * ENTITY_ROTATION_SPEED * tick.delta_time());
        plane.draw(&default_program);

        canvas.on_update_end();
    }
}
