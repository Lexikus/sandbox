#![allow(dead_code)]

use std::collections::HashMap;

use super::keyboard::Input as KeyboardInput;
use super::keyboard::Action;
use super::keyboard::Key;
use super::keyboard::Modifier;

pub struct Input {
    inputs: HashMap<Key, KeyboardInput>,
}

impl Input {
    pub fn new() -> Input {
        let mut collection: HashMap<Key, KeyboardInput> = HashMap::with_capacity(121);

        collection.insert(Key::Space, KeyboardInput { key: Key::Space, action: Action::Release, modifier: Modifier::Unknown });
        collection.insert(Key::Apostrophe, KeyboardInput { key: Key::Apostrophe, action: Action::Release, modifier: Modifier::Unknown });
        collection.insert(Key::Comma, KeyboardInput { key: Key::Comma, action: Action::Release, modifier: Modifier::Unknown });
        collection.insert(Key::Minus, KeyboardInput { key: Key::Minus, action: Action::Release, modifier: Modifier::Unknown });
        collection.insert(Key::Period, KeyboardInput { key: Key::Period, action: Action::Release, modifier: Modifier::Unknown });
        collection.insert(Key::Slash, KeyboardInput { key: Key::Slash, action: Action::Release, modifier: Modifier::Unknown });
        collection.insert(Key::Num0, KeyboardInput { key: Key::Num0, action: Action::Release, modifier: Modifier::Unknown });
        collection.insert(Key::Num1, KeyboardInput { key: Key::Num1, action: Action::Release, modifier: Modifier::Unknown });
        collection.insert(Key::Num2, KeyboardInput { key: Key::Num2, action: Action::Release, modifier: Modifier::Unknown });
        collection.insert(Key::Num3, KeyboardInput { key: Key::Num3, action: Action::Release, modifier: Modifier::Unknown });
        collection.insert(Key::Num4, KeyboardInput { key: Key::Num4, action: Action::Release, modifier: Modifier::Unknown });
        collection.insert(Key::Num5, KeyboardInput { key: Key::Num5, action: Action::Release, modifier: Modifier::Unknown });
        collection.insert(Key::Num6, KeyboardInput { key: Key::Num6, action: Action::Release, modifier: Modifier::Unknown });
        collection.insert(Key::Num7, KeyboardInput { key: Key::Num7, action: Action::Release, modifier: Modifier::Unknown });
        collection.insert(Key::Num8, KeyboardInput { key: Key::Num8, action: Action::Release, modifier: Modifier::Unknown });
        collection.insert(Key::Num9, KeyboardInput { key: Key::Num9, action: Action::Release, modifier: Modifier::Unknown });
        collection.insert(Key::Semicolon, KeyboardInput { key: Key::Semicolon, action: Action::Release, modifier: Modifier::Unknown });
        collection.insert(Key::Equal, KeyboardInput { key: Key::Equal, action: Action::Release, modifier: Modifier::Unknown });
        collection.insert(Key::A, KeyboardInput { key: Key::A, action: Action::Release, modifier: Modifier::Unknown });
        collection.insert(Key::B, KeyboardInput { key: Key::B, action: Action::Release, modifier: Modifier::Unknown });
        collection.insert(Key::C, KeyboardInput { key: Key::C, action: Action::Release, modifier: Modifier::Unknown });
        collection.insert(Key::D, KeyboardInput { key: Key::D, action: Action::Release, modifier: Modifier::Unknown });
        collection.insert(Key::E, KeyboardInput { key: Key::E, action: Action::Release, modifier: Modifier::Unknown });
        collection.insert(Key::F, KeyboardInput { key: Key::F, action: Action::Release, modifier: Modifier::Unknown });
        collection.insert(Key::G, KeyboardInput { key: Key::G, action: Action::Release, modifier: Modifier::Unknown });
        collection.insert(Key::H, KeyboardInput { key: Key::H, action: Action::Release, modifier: Modifier::Unknown });
        collection.insert(Key::I, KeyboardInput { key: Key::I, action: Action::Release, modifier: Modifier::Unknown });
        collection.insert(Key::J, KeyboardInput { key: Key::J, action: Action::Release, modifier: Modifier::Unknown });
        collection.insert(Key::K, KeyboardInput { key: Key::K, action: Action::Release, modifier: Modifier::Unknown });
        collection.insert(Key::L, KeyboardInput { key: Key::L, action: Action::Release, modifier: Modifier::Unknown });
        collection.insert(Key::M, KeyboardInput { key: Key::M, action: Action::Release, modifier: Modifier::Unknown });
        collection.insert(Key::N, KeyboardInput { key: Key::N, action: Action::Release, modifier: Modifier::Unknown });
        collection.insert(Key::O, KeyboardInput { key: Key::O, action: Action::Release, modifier: Modifier::Unknown });
        collection.insert(Key::P, KeyboardInput { key: Key::P, action: Action::Release, modifier: Modifier::Unknown });
        collection.insert(Key::Q, KeyboardInput { key: Key::Q, action: Action::Release, modifier: Modifier::Unknown });
        collection.insert(Key::R, KeyboardInput { key: Key::R, action: Action::Release, modifier: Modifier::Unknown });
        collection.insert(Key::S, KeyboardInput { key: Key::S, action: Action::Release, modifier: Modifier::Unknown });
        collection.insert(Key::T, KeyboardInput { key: Key::T, action: Action::Release, modifier: Modifier::Unknown });
        collection.insert(Key::U, KeyboardInput { key: Key::U, action: Action::Release, modifier: Modifier::Unknown });
        collection.insert(Key::V, KeyboardInput { key: Key::V, action: Action::Release, modifier: Modifier::Unknown });
        collection.insert(Key::W, KeyboardInput { key: Key::W, action: Action::Release, modifier: Modifier::Unknown });
        collection.insert(Key::X, KeyboardInput { key: Key::X, action: Action::Release, modifier: Modifier::Unknown });
        collection.insert(Key::Y, KeyboardInput { key: Key::Y, action: Action::Release, modifier: Modifier::Unknown });
        collection.insert(Key::Z, KeyboardInput { key: Key::Z, action: Action::Release, modifier: Modifier::Unknown });
        collection.insert(Key::LeftBracket, KeyboardInput { key: Key::LeftBracket, action: Action::Release, modifier: Modifier::Unknown });
        collection.insert(Key::Backslash, KeyboardInput { key: Key::Backslash, action: Action::Release, modifier: Modifier::Unknown });
        collection.insert(Key::RightBracket, KeyboardInput { key: Key::RightBracket, action: Action::Release, modifier: Modifier::Unknown });
        collection.insert(Key::GraveAccent, KeyboardInput { key: Key::GraveAccent, action: Action::Release, modifier: Modifier::Unknown });
        collection.insert(Key::World1, KeyboardInput { key: Key::World1, action: Action::Release, modifier: Modifier::Unknown });
        collection.insert(Key::World2, KeyboardInput { key: Key::World2, action: Action::Release, modifier: Modifier::Unknown });
        collection.insert(Key::Escape, KeyboardInput { key: Key::Escape, action: Action::Release, modifier: Modifier::Unknown });
        collection.insert(Key::Enter, KeyboardInput { key: Key::Enter, action: Action::Release, modifier: Modifier::Unknown });
        collection.insert(Key::Tab, KeyboardInput { key: Key::Tab, action: Action::Release, modifier: Modifier::Unknown });
        collection.insert(Key::Backspace, KeyboardInput { key: Key::Backspace, action: Action::Release, modifier: Modifier::Unknown });
        collection.insert(Key::Insert, KeyboardInput { key: Key::Insert, action: Action::Release, modifier: Modifier::Unknown });
        collection.insert(Key::Delete, KeyboardInput { key: Key::Delete, action: Action::Release, modifier: Modifier::Unknown });
        collection.insert(Key::Right, KeyboardInput { key: Key::Right, action: Action::Release, modifier: Modifier::Unknown });
        collection.insert(Key::Left, KeyboardInput { key: Key::Left, action: Action::Release, modifier: Modifier::Unknown });
        collection.insert(Key::Down, KeyboardInput { key: Key::Down, action: Action::Release, modifier: Modifier::Unknown });
        collection.insert(Key::Up, KeyboardInput { key: Key::Up, action: Action::Release, modifier: Modifier::Unknown });
        collection.insert(Key::PageUp, KeyboardInput { key: Key::PageUp, action: Action::Release, modifier: Modifier::Unknown });
        collection.insert(Key::PageDown, KeyboardInput { key: Key::PageDown, action: Action::Release, modifier: Modifier::Unknown });
        collection.insert(Key::Home, KeyboardInput { key: Key::Home, action: Action::Release, modifier: Modifier::Unknown });
        collection.insert(Key::End, KeyboardInput { key: Key::End, action: Action::Release, modifier: Modifier::Unknown });
        collection.insert(Key::CapsLock, KeyboardInput { key: Key::CapsLock, action: Action::Release, modifier: Modifier::Unknown });
        collection.insert(Key::ScrollLock, KeyboardInput { key: Key::ScrollLock, action: Action::Release, modifier: Modifier::Unknown });
        collection.insert(Key::NumLock, KeyboardInput { key: Key::NumLock, action: Action::Release, modifier: Modifier::Unknown });
        collection.insert(Key::PrintScreen, KeyboardInput { key: Key::PrintScreen, action: Action::Release, modifier: Modifier::Unknown });
        collection.insert(Key::Pause, KeyboardInput { key: Key::Pause, action: Action::Release, modifier: Modifier::Unknown });
        collection.insert(Key::F1, KeyboardInput { key: Key::F1, action: Action::Release, modifier: Modifier::Unknown });
        collection.insert(Key::F2, KeyboardInput { key: Key::F2, action: Action::Release, modifier: Modifier::Unknown });
        collection.insert(Key::F3, KeyboardInput { key: Key::F3, action: Action::Release, modifier: Modifier::Unknown });
        collection.insert(Key::F4, KeyboardInput { key: Key::F4, action: Action::Release, modifier: Modifier::Unknown });
        collection.insert(Key::F5, KeyboardInput { key: Key::F5, action: Action::Release, modifier: Modifier::Unknown });
        collection.insert(Key::F6, KeyboardInput { key: Key::F6, action: Action::Release, modifier: Modifier::Unknown });
        collection.insert(Key::F7, KeyboardInput { key: Key::F7, action: Action::Release, modifier: Modifier::Unknown });
        collection.insert(Key::F8, KeyboardInput { key: Key::F8, action: Action::Release, modifier: Modifier::Unknown });
        collection.insert(Key::F9, KeyboardInput { key: Key::F9, action: Action::Release, modifier: Modifier::Unknown });
        collection.insert(Key::F10, KeyboardInput { key: Key::F10, action: Action::Release, modifier: Modifier::Unknown });
        collection.insert(Key::F11, KeyboardInput { key: Key::F11, action: Action::Release, modifier: Modifier::Unknown });
        collection.insert(Key::F12, KeyboardInput { key: Key::F12, action: Action::Release, modifier: Modifier::Unknown });
        collection.insert(Key::F13, KeyboardInput { key: Key::F13, action: Action::Release, modifier: Modifier::Unknown });
        collection.insert(Key::F14, KeyboardInput { key: Key::F14, action: Action::Release, modifier: Modifier::Unknown });
        collection.insert(Key::F15, KeyboardInput { key: Key::F15, action: Action::Release, modifier: Modifier::Unknown });
        collection.insert(Key::F16, KeyboardInput { key: Key::F16, action: Action::Release, modifier: Modifier::Unknown });
        collection.insert(Key::F17, KeyboardInput { key: Key::F17, action: Action::Release, modifier: Modifier::Unknown });
        collection.insert(Key::F18, KeyboardInput { key: Key::F18, action: Action::Release, modifier: Modifier::Unknown });
        collection.insert(Key::F19, KeyboardInput { key: Key::F19, action: Action::Release, modifier: Modifier::Unknown });
        collection.insert(Key::F20, KeyboardInput { key: Key::F20, action: Action::Release, modifier: Modifier::Unknown });
        collection.insert(Key::F21, KeyboardInput { key: Key::F21, action: Action::Release, modifier: Modifier::Unknown });
        collection.insert(Key::F22, KeyboardInput { key: Key::F22, action: Action::Release, modifier: Modifier::Unknown });
        collection.insert(Key::F23, KeyboardInput { key: Key::F23, action: Action::Release, modifier: Modifier::Unknown });
        collection.insert(Key::F24, KeyboardInput { key: Key::F24, action: Action::Release, modifier: Modifier::Unknown });
        collection.insert(Key::F25, KeyboardInput { key: Key::F25, action: Action::Release, modifier: Modifier::Unknown });
        collection.insert(Key::Kp0, KeyboardInput { key: Key::Kp0, action: Action::Release, modifier: Modifier::Unknown });
        collection.insert(Key::Kp1, KeyboardInput { key: Key::Kp1, action: Action::Release, modifier: Modifier::Unknown });
        collection.insert(Key::Kp2, KeyboardInput { key: Key::Kp2, action: Action::Release, modifier: Modifier::Unknown });
        collection.insert(Key::Kp3, KeyboardInput { key: Key::Kp3, action: Action::Release, modifier: Modifier::Unknown });
        collection.insert(Key::Kp4, KeyboardInput { key: Key::Kp4, action: Action::Release, modifier: Modifier::Unknown });
        collection.insert(Key::Kp5, KeyboardInput { key: Key::Kp5, action: Action::Release, modifier: Modifier::Unknown });
        collection.insert(Key::Kp6, KeyboardInput { key: Key::Kp6, action: Action::Release, modifier: Modifier::Unknown });
        collection.insert(Key::Kp7, KeyboardInput { key: Key::Kp7, action: Action::Release, modifier: Modifier::Unknown });
        collection.insert(Key::Kp8, KeyboardInput { key: Key::Kp8, action: Action::Release, modifier: Modifier::Unknown });
        collection.insert(Key::Kp9, KeyboardInput { key: Key::Kp9, action: Action::Release, modifier: Modifier::Unknown });
        collection.insert(Key::KpDecimal, KeyboardInput { key: Key::KpDecimal, action: Action::Release, modifier: Modifier::Unknown });
        collection.insert(Key::KpDivide, KeyboardInput { key: Key::KpDivide, action: Action::Release, modifier: Modifier::Unknown });
        collection.insert(Key::KpMultiply, KeyboardInput { key: Key::KpMultiply, action: Action::Release, modifier: Modifier::Unknown });
        collection.insert(Key::KpSubtract, KeyboardInput { key: Key::KpSubtract, action: Action::Release, modifier: Modifier::Unknown });
        collection.insert(Key::KpAdd, KeyboardInput { key: Key::KpAdd, action: Action::Release, modifier: Modifier::Unknown });
        collection.insert(Key::KpEnter, KeyboardInput { key: Key::KpEnter, action: Action::Release, modifier: Modifier::Unknown });
        collection.insert(Key::KpEqual, KeyboardInput { key: Key::KpEqual, action: Action::Release, modifier: Modifier::Unknown });
        collection.insert(Key::LeftShift, KeyboardInput { key: Key::LeftShift, action: Action::Release, modifier: Modifier::Unknown });
        collection.insert(Key::LeftControl, KeyboardInput { key: Key::LeftControl, action: Action::Release, modifier: Modifier::Unknown });
        collection.insert(Key::LeftAlt, KeyboardInput { key: Key::LeftAlt, action: Action::Release, modifier: Modifier::Unknown });
        collection.insert(Key::LeftSuper, KeyboardInput { key: Key::LeftSuper, action: Action::Release, modifier: Modifier::Unknown });
        collection.insert(Key::RightShift, KeyboardInput { key: Key::RightShift, action: Action::Release, modifier: Modifier::Unknown });
        collection.insert(Key::RightControl, KeyboardInput { key: Key::RightControl, action: Action::Release, modifier: Modifier::Unknown });
        collection.insert(Key::RightAlt, KeyboardInput { key: Key::RightAlt, action: Action::Release, modifier: Modifier::Unknown });
        collection.insert(Key::RightSuper, KeyboardInput { key: Key::RightSuper, action: Action::Release, modifier: Modifier::Unknown });
        collection.insert(Key::Menu, KeyboardInput { key: Key::Menu, action: Action::Release, modifier: Modifier::Unknown });
        collection.insert(Key::Unknown, KeyboardInput { key: Key::Unknown, action: Action::Release, modifier: Modifier::Unknown });

        Input {
            inputs: collection,
        }
    }

    pub fn update(&mut self, key: &Key, input: KeyboardInput) {
        let entry = self.inputs.get_mut(key).unwrap();

        if entry.action == Action::Repeat && input.action == Action::Press {
            return;
        }

        *self.inputs.get_mut(key).unwrap() = input;
    }

    pub fn is_key_pressed(&mut self, key: &Key) -> bool {
        let mut entry = self.inputs.get_mut(key).unwrap();

        if entry.action == Action::Press {
            entry.action = Action::Repeat;
            return true;
        }

        return self.inputs[key].action == Action::Press;
    }

    pub fn is_key_pressed_down(&self, key: &Key) -> bool {
        return self.inputs[key].action == Action::Press || self.inputs[key].action == Action::Repeat;
    }
}
