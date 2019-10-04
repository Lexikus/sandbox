#![allow(dead_code)]

extern crate cgmath as cgm;

use cgm::Vector3;
use cgm::Vector2;
use cgm::prelude::InnerSpace;
use cgm::prelude::Zero;

use std::f32::consts::PI;

use crate::component::entity::Entity;
use crate::graphic::program::Program;

pub struct Sphere {
    entity: Entity,
}

impl Sphere {
    pub fn new(radius: f32, longitude: i32, latitude: i32) -> Sphere {
        let pi = PI;
        let pi_double = pi * 2.0;

        let capacity = ((longitude + 1) * latitude + 2) as usize;

        let mut vertices = Vec::with_capacity(capacity);
        super::fill(&mut vertices, Vector3::<f32>::zero(), capacity);

        vertices[0] = Vector3::unit_y() * radius;
        for lat in 0..latitude {
            let a1 = pi * (lat + 1) as f32 / (latitude + 1) as f32;
            let sin1 = a1.sin();
            let cos1 = a1.cos();

            for long in 0..longitude + 1 {
                let tmp_long = if long == longitude { 0 } else { long };

                let a2 = pi_double * tmp_long as f32 / longitude as f32;
                let sin2 = a2.sin();
                let cos2 = a2.cos();

                vertices[(long + lat * (longitude + 1) + 1) as usize] = Vector3::new(sin1 * cos2, cos1, sin1 * sin2) * radius;
            }
        }
        vertices[capacity - 1] = Vector3::unit_y() * -radius;

        let mut normals = Vec::with_capacity(capacity);
        super::fill(&mut normals, Vector3::<f32>::zero(), capacity);
        for n in 0..capacity {
            normals[n] = vertices[n].normalize();
        }

        let mut uvs = Vec::with_capacity(capacity);
        super::fill(&mut uvs, Vector2::<f32>::zero(), capacity);
        uvs[0] = Vector2::unit_y();
        for lat in 0..latitude {
            for long in 0..longitude + 1 {
                uvs[(long + lat * (longitude + 1) + 1) as usize] = Vector2::new(long as f32 / longitude as f32, 1.0 - (lat as f32 + 1.0) / (latitude as f32 + 1.0));
            }
        }
        uvs[capacity - 1] = Vector2::zero();

        let faces = capacity;
        let triangles_per_face = faces * 2;
        let indices_per_triangle = triangles_per_face * 3;

        let mut indices = Vec::with_capacity(indices_per_triangle);
        super::fill(&mut indices, 0, indices_per_triangle);

        let mut i = 0;

        for long in 0..longitude {
            indices[i] = long + 2;
            i += 1;
            indices[i] = long + 1;
            i += 1;
            indices[i] = 0;
            i += 1;
        }

        for lat in 0..latitude - 1 {
            for long in 0..longitude {
                let current = long + lat * (longitude + 1) + 1;
                let next = current + longitude + 1;

                indices[i] = current;
                i += 1;
                indices[i] = current + 1;
                i += 1;
                indices[i] = next + 1;
                i += 1;

                indices[i] = current;
                i += 1;
                indices[i] = next + 1;
                i += 1;
                indices[i] = next;
                i += 1;
            }
        }

        for long in 0..longitude {
            indices[i] = capacity as i32 - 1;
            i += 1;
            indices[i] = capacity as i32 - (long + 2) - 1;
            i += 1;
            indices[i] = capacity as i32 - (long + 1) - 1;
            i += 1;
        }

        let mut data =
            Vec::with_capacity(capacity * 3 + capacity * 3 + capacity * 2 + capacity * 4);

        for (i, vertex) in vertices.iter().enumerate() {
            data.push(vertex.x);
            data.push(vertex.y);
            data.push(vertex.z);

            let normal = normals[i];
            data.push(normal.x);
            data.push(normal.y);
            data.push(normal.z);

            let uv = uvs[i];
            data.push(uv.x);
            data.push(uv.y);

            // color
            for _ in 0..4 {
                data.push(1.0);
            }
        }

        Sphere {
            entity: Entity::new(data, indices),
        }
    }

    pub fn entity(&self) -> &Entity {
        &self.entity
    }

    pub fn entity_mut(&mut self) -> &mut Entity {
        &mut self.entity
    }

    pub fn translate(&mut self, translation: Vector3<f32>) {
        self.entity.transform_mut().translate(translation);
    }

    pub fn rotate_y(&mut self, rotate_in_deg: f32) {
        self.entity.transform_mut().rotate_y(rotate_in_deg);
    }

    pub fn draw(&self, program: &Program) {
        program.set_mat4f("model", self.entity.transform().matrix());
        program.set_mat4f("uTangentToWorld", &self.entity.transform().matrix_tangent());
        self.entity.mesh().bind();

        unsafe {
            gl::DrawElements(gl::TRIANGLES, 1000, gl::UNSIGNED_INT, std::ptr::null());
        }
    }
}
