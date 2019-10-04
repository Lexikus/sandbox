#![allow(dead_code)]

extern crate cgmath as cgm;

pub enum CameraType {
    Perspective,
    Orthographic,
}

pub struct Camera {
    camera_type: CameraType,
    projection_matrix: cgm::Matrix4<f32>,
    view_matrix: cgm::Matrix4<f32>,
    view_projection_matrix: cgm::Matrix4<f32>,
    position: cgm::Vector3<f32>,
}

impl Camera {
    pub fn perspective(fovy_deg: f32, aspect: f32, near: f32, far: f32) -> Camera {
        if near <= 0.0 || far <= 0.0 {
            panic!("near or far needs to greater than zero");
        }

        let projection = cgm::perspective(cgm::Deg(fovy_deg), aspect, near, far);
        let mut position: cgm::Vector3<f32> = cgm::Vector3::new(0.0, -2.0, 0.0);
        let mut view: cgm::Matrix4<f32> = cgm::Matrix4::from_translation(position);
        view = view * cgm::Matrix4::<f32>::from_axis_angle(cgm::Vector3::new(1.0, 0.0, 0.0), cgm::Deg(20.0));
        position = cgm::Vector3::new(view.x.x, view.y.y, view.z.z);

        Camera {
            camera_type: CameraType::Perspective,
            projection_matrix: projection,
            view_matrix: view,
            view_projection_matrix: projection * view,
            position: position,
        }
    }

    pub fn position(&self) -> &cgm::Vector3<f32> {
        &self.position
    }

    pub fn get_projection(&self) -> &cgm::Matrix4<f32> {
        &self.projection_matrix
    }

    pub fn get_view(&self) -> &cgm::Matrix4<f32> {
        &self.view_matrix
    }

    pub fn get_view_projection(&self) -> &cgm::Matrix4<f32> {
        &self.view_projection_matrix
    }

    pub fn calculate_matrices(&mut self) {
        self.view_projection_matrix = self.projection_matrix * self.view_matrix;
    }

    pub fn translate(&mut self, translation: cgm::Vector3<f32>) {
        let translation = cgm::Matrix4::<f32>::from_translation(translation);
        self.view_matrix = self.view_matrix * translation;
        self.position = cgm::Vector3::new(self.view_matrix.x.x, self.view_matrix.y.y, self.view_matrix.z.z);
        self.calculate_matrices();
    }

    pub fn rotate_x(&mut self, rotate_in_deg: f32) {
        let rotation = cgm::Matrix4::<f32>::from_angle_x(cgm::Deg(rotate_in_deg));
        self.view_matrix = self.view_matrix * rotation;
        self.position = cgm::Vector3::new(self.view_matrix.x.x, self.view_matrix.y.y, self.view_matrix.z.z);
        self.calculate_matrices();
    }

    pub fn rotate_y(&mut self, rotate_in_deg: f32) {
        let rotation = cgm::Matrix4::<f32>::from_angle_y(cgm::Deg(rotate_in_deg));
        self.view_matrix = self.view_matrix * rotation;
        self.position = cgm::Vector3::new(self.view_matrix.x.x, self.view_matrix.y.y, self.view_matrix.z.z);
        self.calculate_matrices();
    }

    pub fn rotate_z(&mut self, rotate_in_deg: f32) {
        let rotation = cgm::Matrix4::<f32>::from_angle_z(cgm::Deg(rotate_in_deg));
        self.view_matrix = self.view_matrix * rotation;
        self.position = cgm::Vector3::new(self.view_matrix.x.x, self.view_matrix.y.y, self.view_matrix.z.z);
        self.calculate_matrices();
    }
}