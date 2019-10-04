#version 400 core

out vec4 fragColor;

in vec3 vUV;

uniform samplerCube skybox;

void main()
{
    fragColor = texture(skybox, vUV);
}