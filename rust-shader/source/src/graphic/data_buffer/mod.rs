#![allow(dead_code)]

extern crate cgmath;
extern crate gl;
pub mod buffer_element;

use buffer_element::BufferElement;

pub struct DataBuffer {
    id: u32,
    stride: i32,
    elements: Vec<BufferElement>,
}

impl DataBuffer {
    pub fn new<T>(data: *const T, size: usize) -> DataBuffer {
        let mut id: u32 = 0;

        unsafe {
            gl::GenBuffers(1, &mut id);
            gl::BindBuffer(gl::ARRAY_BUFFER, id);
            gl::BufferData(
                gl::ARRAY_BUFFER,
                size as isize,
                data as *const std::ffi::c_void,
                gl::STATIC_DRAW,
            );
        }

        DataBuffer {
            id: id,
            stride: 0,
            elements: Vec::new(),
        }
    }

    pub fn bind(&self) {
        unsafe {
            gl::BindBuffer(gl::ARRAY_BUFFER, self.id);
        }
    }

    pub fn unbind(&self) {
        unsafe {
            gl::BindBuffer(gl::ARRAY_BUFFER, 0);
        }
    }

    pub fn add_element(&mut self, element: BufferElement) {
        self.stride += element.size();
        self.elements.push(element);
    }

    pub fn configure_by_index(&self) {
        unsafe {
            gl::BindBuffer(gl::ARRAY_BUFFER, self.id);
        };

        let mut offset: i32 = 0;

        for (i, element) in self.elements.iter().enumerate() {
            unsafe {
                gl::VertexAttribPointer(
                    i as u32,
                    element.count(),
                    element.api_type(),
                    element.normalized(),
                    self.stride,
                    offset as *const std::ffi::c_void,
                );
                gl::EnableVertexAttribArray(i as u32);
            }

            offset += element.size();
        }
    }

    pub fn configure_by_name(&self, program_id: u32) {
        unsafe {
            gl::BindBuffer(gl::ARRAY_BUFFER, self.id);
        };

        let mut offset: i32 = 0;

        for element in self.elements.iter() {
            let position: i32 = unsafe {
                gl::GetAttribLocation(
                    program_id,
                    element.name().as_ptr() as *const gl::types::GLchar,
                )
            };

            unsafe {
                gl::VertexAttribPointer(
                    position as u32,
                    element.count(),
                    element.api_type(),
                    element.normalized(),
                    self.stride,
                    offset as *const std::ffi::c_void,
                );
                gl::EnableVertexAttribArray(position as u32);
            }

            offset += element.size();
        }
    }
}
