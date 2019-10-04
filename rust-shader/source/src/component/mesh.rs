#![allow(dead_code)]

use crate::graphic::data_buffer::buffer_element::BufferDataType;
use crate::graphic::data_buffer::buffer_element::BufferElement;
use crate::graphic::data_buffer::DataBuffer;
use crate::graphic::index_buffer::IndexBuffer;
use crate::graphic::vertex_array::VertexArray;

pub struct Mesh {
    vertex_array: VertexArray,
    data_buffer: DataBuffer,
    index_buffer: IndexBuffer,
    data: Vec<f32>,
    indices: Vec<i32>,
}

impl Mesh {
    pub fn new(data: Vec<f32>, indices: Vec<i32>) -> Mesh {
        let vertex_array = VertexArray::new();
        vertex_array.bind();

        let mut data_buffer = DataBuffer::new(data.as_ptr(), data.len() * std::mem::size_of::<f32>());

        let buffer_element_position = BufferElement::new(BufferDataType::Float3, "aPos", false);
        data_buffer.add_element(buffer_element_position);

        let buffer_element_normal = BufferElement::new(BufferDataType::Float3, "aNor", false);
        data_buffer.add_element(buffer_element_normal);

        let buffer_element_uv = BufferElement::new(BufferDataType::Float2, "aUV", false);
        data_buffer.add_element(buffer_element_uv);

        let buffer_element_color = BufferElement::new(BufferDataType::Float4, "aCol", false);
        data_buffer.add_element(buffer_element_color);
        data_buffer.configure_by_index();

        let index_buffer = IndexBuffer::new(indices.as_ptr(), indices.len() * std::mem::size_of::<i32>());

        vertex_array.unbind();
        data_buffer.unbind();
        index_buffer.unbind();

        Mesh {
            vertex_array: vertex_array,
            data_buffer: data_buffer,
            index_buffer: index_buffer,
            data: data,
            indices: indices,
        }
    }

    pub fn bind(&self) {
        self.vertex_array.bind();
    }

    pub fn unbind(&self) {
        self.vertex_array.unbind();
    }
}