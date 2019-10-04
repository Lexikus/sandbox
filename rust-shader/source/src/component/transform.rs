#![allow(dead_code)]

extern crate cgmath as cgm;
use cgm::prelude::Zero;
use cgm::prelude::SquareMatrix;
use cgm::prelude::Matrix;

use cgm::Vector3;
use cgm::Matrix4;
use cgm::Quaternion;

pub struct Transform {
    position: Vector3<f32>,
    rotation: Quaternion<f32>,
    scale: Vector3<f32>,
    matrix: Matrix4<f32>
}

impl Transform {
    pub fn new() -> Transform {
        Transform {
            position: Vector3::zero(),
            rotation: Quaternion::zero(),
            scale: Vector3::new(1.0, 1.0, 1.0),
            matrix: Matrix4::from_scale(1.0) * Matrix4::from_translation(Vector3::zero()),
        }
    }

    pub fn position(&self) -> &Vector3<f32> {
        &self.position
    }

    pub fn rotation(&self) -> &Quaternion<f32> {
        &self.rotation
    }

    pub fn scale(&self) -> &Vector3<f32> {
        &self.scale
    }

    pub fn matrix(&self) -> &Matrix4<f32> {
        &self.matrix
    }

    pub fn matrix_tangent(&self) -> Matrix4<f32> {
        self.matrix.invert().unwrap().transpose()
    }

    pub fn translate(&mut self, translation: Vector3<f32>) {
        let translation = Matrix4::from_translation(translation);
        self.matrix = self.matrix * translation;
        self.position = Vector3::new(self.matrix.x.x, self.matrix.y.y, self.matrix.z.z);
    }

    pub fn rotate_x(&mut self, rotate_in_deg: f32) {
        self.matrix = self.matrix * cgm::Matrix4::<f32>::from_angle_x(cgm::Deg(rotate_in_deg));
    }

    pub fn rotate_y(&mut self, rotate_in_deg: f32) {
        self.matrix = self.matrix * cgm::Matrix4::<f32>::from_angle_y(cgm::Deg(rotate_in_deg));
    }

    pub fn rotate_z(&mut self, rotate_in_deg: f32) {
        self.matrix = self.matrix * cgm::Matrix4::<f32>::from_angle_z(cgm::Deg(rotate_in_deg));
    }
}