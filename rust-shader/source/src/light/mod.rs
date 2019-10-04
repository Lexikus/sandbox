#![allow(dead_code)]

extern crate cgmath as cgm;

use cgm::Vector3;
use cgm::Matrix4;

pub struct Light {
    position: Vector3<f32>,
    ambient_light_color: Vector3<f32>,
    light_color: Vector3<f32>,
    ambient_intensity: f32,
    ambient_factor: f32,
    diffuse_intensity: f32,
    diffuse_factor: f32,
    specular_intensity: f32,
    specular_factor: f32,
    hardness: f32,
}

impl Light {
    pub fn new(
        position: Vector3<f32>,
        ambient_light_color: Vector3<f32>,
        light_color: Vector3<f32>,
        ambient_intensity: f32,
        ambient_factor: f32,
        diffuse_intensity: f32,
        diffuse_factor: f32,
        specular_intensity: f32,
        specular_factor: f32,
        hardness: f32,
    ) -> Light {
        Light {
            position: position,
            ambient_light_color: ambient_light_color,
            light_color: light_color,
            ambient_intensity: ambient_intensity,
            ambient_factor: ambient_factor,
            diffuse_intensity: diffuse_intensity,
            diffuse_factor: diffuse_factor,
            specular_intensity: specular_intensity,
            specular_factor: specular_factor,
            hardness: hardness
        }
    }

    pub fn position(&self) -> Vector3<f32> {
        self.position
    }

    pub fn ambient_light_color(&self) -> Vector3<f32> {
        self.ambient_light_color
    }

    pub fn set_ambient_light_color(&mut self, color: Vector3<f32>) {
        self.ambient_light_color = color;
    }

    pub fn light_color(&self) -> Vector3<f32> {
        self.light_color
    }

    pub fn set_light_color(&mut self, color: Vector3<f32>) {
        self.light_color = color;
    }

    pub fn ambient_intensity(&self) -> f32 {
        self.ambient_intensity
    }

    pub fn ambient_factor(&self) -> f32 {
        self.ambient_factor
    }

    pub fn diffuse_intensity(&self) -> f32 {
        self.diffuse_intensity
    }

    pub fn diffuse_factor(&self) -> f32 {
        self.diffuse_factor
    }

    pub fn specular_intensity(&self) -> f32 {
        self.specular_intensity
    }

    pub fn specular_factor(&self) -> f32 {
        self.specular_factor
    }

    pub fn hardness(&self) -> f32 {
        self.hardness
    }

    pub fn as_matrix(&self) -> Matrix4<f32> {
        Matrix4::new(
            self.position.x, self.position.y, self.position.z, self.ambient_intensity,
            self.ambient_light_color.x, self.ambient_light_color.y, self.ambient_light_color.z, self.diffuse_intensity,
            self.light_color.x, self.light_color.y, self.light_color.z, self.specular_intensity,
            self.ambient_factor, self.diffuse_factor, self.specular_factor, self.hardness,
        )
    }

    pub fn add_to_position(&mut self, translation: Vector3<f32>) {
        self.position += translation;
    }
}
