#![allow(dead_code)]
extern crate gl;
extern crate image;

use image::GenericImageView;

pub enum TextureError {
    OpeningTextureFailed,
}

pub struct Texture {
    id: u32,
    width: u32,
    height: u32,
}

impl Texture {
    pub fn new(path: &str) -> Result<Texture, TextureError> {
        let texture = match image::open(path) {
            Ok(texture) => texture,
            Err(_) => return Err(TextureError::OpeningTextureFailed),
        };

        let format = match texture {
            image::ImageLuma8(_) => gl::RED,
            image::ImageLumaA8(_) => gl::RG,
            image::ImageRgb8(_) => gl::RGB,
            image::ImageRgba8(_) => gl::RGBA,
            _ => gl::RGB,
        };

        let mut id: u32 = 0;

        unsafe {
            gl::GenTextures(1, &mut id);
            gl::BindTexture(gl::TEXTURE_2D, id);
            gl::TexImage2D(gl::TEXTURE_2D, 0, format as i32, texture.width() as i32, texture.height() as i32, 0, gl::RGB, gl::UNSIGNED_BYTE, texture.raw_pixels().as_ptr() as *const std::ffi::c_void);
            gl::GenerateMipmap(gl::TEXTURE_2D);
        }

        Ok(Texture {
            id: id,
            width: 0,
            height: 0,
        })
    }

    pub fn bind(&self) {
        unsafe {
            gl::BindTexture(gl::TEXTURE_2D, self.id);
        }
    }

    pub fn bind_at_position(&self, position: u32) {
        unsafe {
            gl::ActiveTexture(gl::TEXTURE0 as u32 + position);
            gl::BindTexture(gl::TEXTURE_2D, self.id);
        }
    }

    pub fn unbind(&self) {
        unsafe {
            gl::BindTexture(gl::TEXTURE_2D, 0);
        }
    }
}
