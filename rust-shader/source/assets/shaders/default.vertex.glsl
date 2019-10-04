#version 400 core

layout (location = 0) in vec3 aPos;
layout (location = 1) in vec3 aNor;
layout (location = 2) in vec2 aUV;
layout (location = 3) in vec4 aCol;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;

uniform mat4 uTangentToWorld;

out vec4 vCol;
out vec2 vUV;
out vec3 vNor;
out vec3 vPos;

void main() {
    vCol = aCol;
    vUV = aUV;
    vNor = mat3(uTangentToWorld) * aNor;
    vPos = vec3(model * vec4(aPos, 1.0f));
    gl_Position = projection * view * model * vec4(aPos, 1.0f);
}