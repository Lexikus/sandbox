pub mod cube;
pub mod plane;
pub mod sphere;
pub mod sky_box;

fn fill<T: Copy>(vector: &mut Vec<T>, filler: T, capacity: usize) {
    for _ in 0..capacity {
        vector.push(filler);
    }
}