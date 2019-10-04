#![allow(dead_code)]
extern crate gl;

use std::ffi::CString;
use std::fs::File;
use std::io::Read;
use std::ptr;

pub enum ShaderError {
    FailedOpeningFile,
    FailedReadingFile,
    FailedCompilingShader(String),
}

pub enum ShaderType {
    VertexShader,
    FragmentShader,
}

pub struct Shader {
    id: u32,
}

impl Shader {
    pub fn new(path: &str, shader_type: ShaderType) -> Result<Shader, ShaderError> {
        let mut shader_file = match File::open(path) {
            Ok(file) => file,
            Err(_) => return Err(ShaderError::FailedOpeningFile),
        };

        let mut shader_code = String::new();
        match shader_file.read_to_string(&mut shader_code) {
            Ok(number) => number,
            Err(_) => return Err(ShaderError::FailedReadingFile),
        };

        let shader = CString::new(shader_code.as_bytes()).unwrap();

        let id: u32 = unsafe {
            let id = match shader_type {
                ShaderType::VertexShader => gl::CreateShader(gl::VERTEX_SHADER),
                ShaderType::FragmentShader => gl::CreateShader(gl::FRAGMENT_SHADER),
            };

            gl::ShaderSource(id, 1, &shader.as_ptr(), ptr::null());

            id
        };

        let success = unsafe {
            let mut success = 0;
            gl::CompileShader(id);
            gl::GetShaderiv(id, gl::COMPILE_STATUS, &mut success);
            success
        };

        if success == 0 {
            let error_message = unsafe {
                let mut len = 0;
                gl::GetShaderiv(id, gl::INFO_LOG_LENGTH, &mut len);

                let mut buf = Vec::with_capacity(len as usize);
                let buf_ptr = buf.as_mut_ptr() as *mut gl::types::GLchar;

                buf.set_len(len as usize);
                gl::GetShaderInfoLog(id, len, std::ptr::null_mut(), buf_ptr);

                let error_message = match String::from_utf8(buf) {
                    Ok(log) => log,
                    Err(vec) => panic!("Could not convert compilation log from buffer: {}", vec)
                };

                error_message
            };

            return Err(ShaderError::FailedCompilingShader(error_message));
        }

        Ok(Shader { id: id })
    }

    pub fn id(&self) -> u32 {
        self.id
    }

    pub fn delete(&self) {
        unsafe { gl::DeleteShader(self.id) };
    }
}