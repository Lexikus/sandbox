#![allow(dead_code)]
extern crate gl;


use std::ffi::CStr;
use std::ffi::CString;
pub enum BufferDataType {
    None,
    Float,
    Float2,
    Float3,
    Float4,
    Mat3,
    Mat4,
    Int,
    Int2,
    Int3,
    Int4,
    Bool,
}

impl BufferDataType {
    pub fn size(&self) -> i32 {
        match self {
            BufferDataType::None => 0,
            BufferDataType::Float => 4,
            BufferDataType::Float2 => 4 * 2,
            BufferDataType::Float3 => 4 * 3,
            BufferDataType::Float4 => 4 * 4,
            BufferDataType::Mat3 => 4 * 3 * 3,
            BufferDataType::Mat4 => 4 * 4 * 4,
            BufferDataType::Int => 4,
            BufferDataType::Int2 => 4 * 2,
            BufferDataType::Int3 => 4 * 3,
            BufferDataType::Int4 => 4 * 4,
            BufferDataType::Bool => 1,
        }
    }

    pub fn api_type(&self) -> u32 {
        match self {
            BufferDataType::None => 0,
            BufferDataType::Float => gl::FLOAT,
            BufferDataType::Float2 => gl::FLOAT,
            BufferDataType::Float3 => gl::FLOAT,
            BufferDataType::Float4 => gl::FLOAT,
            BufferDataType::Mat3 => gl::FLOAT,
            BufferDataType::Mat4 => gl::FLOAT,
            BufferDataType::Int => gl::INT,
            BufferDataType::Int2 => gl::INT,
            BufferDataType::Int3 => gl::INT,
            BufferDataType::Int4 => gl::INT,
            BufferDataType::Bool => gl::BOOL,
        }
    }

    pub fn count(&self) -> i32 {
        match self {
            BufferDataType::None => 0,
            BufferDataType::Float => 1,
            BufferDataType::Float2 => 2,
            BufferDataType::Float3 => 3,
            BufferDataType::Float4 => 4,
            BufferDataType::Mat3 => 3 * 3,
            BufferDataType::Mat4 => 4 * 4,
            BufferDataType::Int => 1,
            BufferDataType::Int2 => 2,
            BufferDataType::Int3 => 3,
            BufferDataType::Int4 => 4,
            BufferDataType::Bool => 1,
        }
    }
}

pub struct BufferElement {
    name: CString,
    size: i32,
    count: i32,
    api_type: u32,
    normalized: u8,
}

impl BufferElement {
    pub fn new(buffer_data_type: BufferDataType, name: &str, normalized: bool) -> BufferElement {
        BufferElement {
            name: CString::new(name.as_bytes()).unwrap(),
            size: buffer_data_type.size(),
            count: buffer_data_type.count(),
            api_type: buffer_data_type.api_type(),
            normalized: match normalized {
                true => 1,
                false => 0,
            },
        }
    }

    pub fn name(&self) -> &CStr {
        &self.name
    }

    pub fn size(&self) -> i32 {
        self.size
    }

    pub fn count(&self) -> i32 {
        self.count
    }

    pub fn api_type(&self) -> u32 {
        self.api_type
    }

    pub fn normalized(&self) -> u8 {
        self.normalized
    }
}