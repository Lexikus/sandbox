#![allow(dead_code)]

extern crate gl;

use crate::graphic::data_buffer::buffer_element::BufferDataType;
use crate::graphic::data_buffer::buffer_element::BufferElement;
use crate::graphic::data_buffer::DataBuffer;
use crate::graphic::vertex_array::VertexArray;

pub struct SkyBox {
    vertex_array: VertexArray,
    data_buffer: DataBuffer,
    data: Vec<f32>,
}

impl SkyBox {
    pub fn new() -> SkyBox {
        let data: Vec<f32> = vec!(
            -1.0,  1.0, -1.0,
            -1.0, -1.0, -1.0,
             1.0, -1.0, -1.0,
             1.0, -1.0, -1.0,
             1.0,  1.0, -1.0,
            -1.0,  1.0, -1.0,
            -1.0, -1.0,  1.0,
            -1.0, -1.0, -1.0,
            -1.0,  1.0, -1.0,
            -1.0,  1.0, -1.0,
            -1.0,  1.0,  1.0,
            -1.0, -1.0,  1.0,
             1.0, -1.0, -1.0,
             1.0, -1.0,  1.0,
             1.0,  1.0,  1.0,
             1.0,  1.0,  1.0,
             1.0,  1.0, -1.0,
             1.0, -1.0, -1.0,
            -1.0, -1.0,  1.0,
            -1.0,  1.0,  1.0,
             1.0,  1.0,  1.0,
             1.0,  1.0,  1.0,
             1.0, -1.0,  1.0,
            -1.0, -1.0,  1.0,
            -1.0,  1.0, -1.0,
             1.0,  1.0, -1.0,
             1.0,  1.0,  1.0,
             1.0,  1.0,  1.0,
            -1.0,  1.0,  1.0,
            -1.0,  1.0, -1.0,
            -1.0, -1.0, -1.0,
            -1.0, -1.0,  1.0,
             1.0, -1.0, -1.0,
             1.0, -1.0, -1.0,
            -1.0, -1.0,  1.0,
             1.0, -1.0,  1.0,
        );

        let vertex_array = VertexArray::new();
        vertex_array.bind();

        let mut data_buffer = DataBuffer::new(data.as_ptr(), data.len() * std::mem::size_of::<f32>());

        let buffer_element_position = BufferElement::new(BufferDataType::Float3, "aPos", false);
        data_buffer.add_element(buffer_element_position);
        data_buffer.configure_by_index();

        vertex_array.unbind();
        data_buffer.unbind();

        SkyBox {
            vertex_array: vertex_array,
            data_buffer: data_buffer,
            data: data,
        }
    }

    pub fn draw(&self) {
        self.vertex_array.bind();

        unsafe {
            gl::DrawArrays(gl::TRIANGLES, 0, 36);
        }
    }
}