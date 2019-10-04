#![allow(dead_code)]

use std::time::SystemTime;

pub struct Tick {
    time_until_start: SystemTime,
    previous_time: SystemTime,
    delta_time: f32,
}

impl Tick {
    pub fn new() -> Tick {
        Tick {
            time_until_start: SystemTime::now(),
            previous_time: SystemTime::now(),
            delta_time: 0.0,
        }
    }

    pub fn on_update(&mut self) {
        let time = SystemTime::now();

        self.delta_time = match time.duration_since(self.previous_time) {
            Ok(time) => time.as_millis() as f32 / 1000.0,
            Err(_) => 0.0,
        };

        self.previous_time = SystemTime::now();
    }

    pub fn delta_time(&self) -> f32 {
        self.delta_time
    }

    pub fn time(&self) -> f32 {
        let time = SystemTime::now();
        match time.duration_since(self.time_until_start) {
            Ok(time) => time.as_secs() as f32,
            Err(_) => return 0.0,
        }
    }
}