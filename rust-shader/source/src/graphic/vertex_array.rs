#![allow(dead_code)]
extern crate gl;

pub struct VertexArray {
    id: u32,
}

impl VertexArray {
    pub fn new() -> VertexArray {
        let mut id: u32 = 0;
        unsafe { gl::GenVertexArrays(1, &mut id) };

        VertexArray { id: id }
    }

    pub fn bind(&self) {
        unsafe {
            gl::BindVertexArray(self.id);
        };
    }

    pub fn unbind(&self) {
        unsafe {
            gl::BindVertexArray(0);
        };
    }
}