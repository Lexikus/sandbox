#![allow(dead_code)]
extern crate gl;

pub struct IndexBuffer {
    id: u32,
}

impl IndexBuffer {
    pub fn new<T>(data: *const T, size: usize) -> IndexBuffer {
        let mut id: u32 = 0;

        unsafe {
            gl::GenBuffers(1, &mut id);
        };

        unsafe {
            gl::BindBuffer(gl::ELEMENT_ARRAY_BUFFER, id);
            gl::BufferData(
                gl::ELEMENT_ARRAY_BUFFER,
                size as isize,
                data as *const std::ffi::c_void,
                gl::STATIC_DRAW,
            );
        };

        IndexBuffer { id: id }
    }

    pub fn bind(&self) {
        unsafe {
            gl::BindBuffer(gl::ELEMENT_ARRAY_BUFFER, self.id);
        }
    }

    pub fn unbind(&self) {
        unsafe {
            gl::BindBuffer(gl::ELEMENT_ARRAY_BUFFER, 0);
        }
    }
}