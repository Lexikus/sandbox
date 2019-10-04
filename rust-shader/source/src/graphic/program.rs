#![allow(dead_code)]

extern crate cgmath;
extern crate gl;
use super::shader::Shader;
use std::ffi::CString;


use cgmath::prelude::Array;
use cgmath::prelude::Matrix;
use cgmath::{Matrix3, Matrix4, Vector2, Vector3, Vector4};
pub enum ProgramError {
    FailedLinkingShader(String),
}

pub struct Program {
    id: u32,
}

impl Program {
    pub fn new(vertex_shader: Shader, fragment_shader: Shader) -> Result<Program, ProgramError> {
        let id = unsafe { gl::CreateProgram() };

        let success = unsafe {
            let mut success = 0;

            gl::AttachShader(id, vertex_shader.id());
            gl::AttachShader(id, fragment_shader.id());
            gl::LinkProgram(id);

            gl::GetProgramiv(id, gl::LINK_STATUS, &mut success);

            success
        };

        if success == 0 {
            let error_message = unsafe {
                let mut len = 0;
                gl::GetProgramiv(id, gl::INFO_LOG_LENGTH, &mut len);

                let mut buf = Vec::with_capacity(len as usize);
                let buf_ptr = buf.as_mut_ptr() as *mut gl::types::GLchar;

                buf.set_len(len as usize);
                gl::GetProgramInfoLog(id, len, std::ptr::null_mut(), buf_ptr);

                let error_message = match String::from_utf8(buf) {
                    Ok(log) => log,
                    Err(vec) => panic!("Could not convert compilation log from buffer: {}", vec),
                };

                error_message
            };
            return Err(ProgramError::FailedLinkingShader(error_message));
        }

        vertex_shader.delete();
        fragment_shader.delete();

        Ok(Program { id: id })
    }

    pub fn id(&self) -> u32 {
        self.id
    }

    pub fn bind(&self) {
        unsafe {
            gl::UseProgram(self.id);
        }
    }

    pub fn unbind(&self) {
        unsafe {
            gl::UseProgram(0);
        }
    }

    pub fn set_bool(&self, name: &str, value: bool) {
        let name = CString::new(name).unwrap();
        let value = match value {
            true => 1,
            false => 0,
        };

        unsafe {
            let uniform_location = gl::GetUniformLocation(self.id, name.as_ptr());
            gl::Uniform1i(uniform_location, value);
        };
    }

    pub fn set_int(&self, name: &str, value: i32) {
        let name = CString::new(name).unwrap();

        unsafe {
            let uniform_location = gl::GetUniformLocation(self.id, name.as_ptr());
            gl::Uniform1i(uniform_location, value);
        }
    }

    pub fn set_float(&self, name: &str, value: f32) {
        let name = CString::new(name).unwrap();

        unsafe {
            let uniform_location = gl::GetUniformLocation(self.id, name.as_ptr());
            gl::Uniform1f(uniform_location, value);
        };
    }

    pub fn set_vec2f(&self, name: &str, value: &Vector2<f32>) {
        let name = CString::new(name).unwrap();

        unsafe {
            let uniform_location = gl::GetUniformLocation(self.id, name.as_ptr());
            gl::Uniform2fv(uniform_location, 1, value.as_ptr());
        };
    }

    pub fn set_vec3f(&self, name: &str, value: &Vector3<f32>) {
        let name = CString::new(name).unwrap();

        unsafe {
            let uniform_location = gl::GetUniformLocation(self.id, name.as_ptr());
            gl::Uniform3fv(uniform_location, 1, value.as_ptr());
        };
    }

    pub fn set_vec4f(&self, name: &str, value: &Vector4<f32>) {
        let name = CString::new(name).unwrap();

        unsafe {
            let uniform_location = gl::GetUniformLocation(self.id, name.as_ptr());
            gl::Uniform4fv(uniform_location, 1, value.as_ptr());
        };
    }

    pub fn set_mat3f(&self, name: &str, value: &Matrix3<f32>) {
        let name = CString::new(name).unwrap();

        unsafe {
            let uniform_location = gl::GetUniformLocation(self.id, name.as_ptr());
            gl::UniformMatrix3fv(uniform_location, 1, 0, value.as_ptr());
        };
    }

    pub fn set_mat4f(&self, name: &str, value: &Matrix4<f32>) {
        let name = CString::new(name).unwrap();

        unsafe {
            let uniform_location = gl::GetUniformLocation(self.id, name.as_ptr());
            gl::UniformMatrix4fv(uniform_location, 1, 0, value.as_ptr());
        };
    }
}
