#![allow(dead_code)]

use super::transform::Transform;
use super::mesh::Mesh;

pub struct Entity {
    transform: Transform,
    mesh: Mesh,
}

impl Entity {
    pub fn new(data: Vec<f32>, indices: Vec<i32>) -> Entity {
        Entity {
            transform: Transform::new(),
            mesh: Mesh::new(data, indices),
        }
    }

    pub fn transform(&self) -> &Transform {
        &self.transform
    }

    pub fn transform_mut(&mut self) -> &mut Transform {
        &mut self.transform
    }

    pub fn mesh(&self) -> &Mesh {
        &self.mesh
    }
}