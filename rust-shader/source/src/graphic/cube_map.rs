#![allow(dead_code)]
extern crate gl;
extern crate image;

use image::GenericImageView;

pub enum CubeMapError {
    OpeningTextureFailed(String),
}

pub struct CubeMap {
    id: u32,
}

impl CubeMap {
    pub fn new(
        right: &str,
        left: &str,
        top: &str,
        bottom: &str,
        back: &str,
        front: &str,
    ) -> Result<CubeMap, CubeMapError> {

        let collection = [right, left, top, bottom, back, front];

        let mut textures = Vec::with_capacity(collection.len());

        for texture in collection.iter() {
            let texture = match image::open(texture) {
                Ok(texture) => texture,
                Err(_) => {
                    return Err(CubeMapError::OpeningTextureFailed(String::from(
                        format!("opening cube map texture {} failed", texture),
                    )));
                }
            };

            textures.push(texture);
        }

        let mut id: u32 = 0;

        unsafe {
            gl::GenTextures(1, &mut id);
            gl::BindTexture(gl::TEXTURE_CUBE_MAP, id);
        };

        for (index, texture) in textures.iter().enumerate() {
            unsafe {
                gl::TexImage2D(
                    gl::TEXTURE_CUBE_MAP_POSITIVE_X as u32 + index as u32,
                    0,
                    gl::RGB as i32,
                    texture.width() as i32,
                    texture.height() as i32,
                    0,
                    gl::RGB,
                    gl::UNSIGNED_BYTE,
                    texture.raw_pixels().as_ptr() as *const std::ffi::c_void,
                );
            }
        }

        unsafe {
            gl::TexParameteri(gl::TEXTURE_CUBE_MAP, gl::TEXTURE_MAG_FILTER, gl::LINEAR as i32);
            gl::TexParameteri(gl::TEXTURE_CUBE_MAP, gl::TEXTURE_MIN_FILTER, gl::LINEAR as i32);
            gl::TexParameteri(gl::TEXTURE_CUBE_MAP, gl::TEXTURE_WRAP_S, gl::CLAMP_TO_EDGE as i32);
            gl::TexParameteri(gl::TEXTURE_CUBE_MAP, gl::TEXTURE_WRAP_T, gl::CLAMP_TO_EDGE as i32);
            gl::TexParameteri(gl::TEXTURE_CUBE_MAP, gl::TEXTURE_WRAP_R, gl::CLAMP_TO_EDGE as i32);
        }

        Ok(CubeMap { id: id })
    }

    pub fn bind(&self) {
        unsafe {
            gl::BindTexture(gl::TEXTURE_CUBE_MAP, self.id);
        }
    }

    pub fn unbind(&self) {
        unsafe {
            gl::BindTexture(gl::TEXTURE_CUBE_MAP, 0);
        }
    }
}
