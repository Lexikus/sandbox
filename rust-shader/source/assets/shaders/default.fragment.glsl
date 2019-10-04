#version 400 core

uniform mat4 uLight;
uniform float uBrightness;
uniform float uContrast;
uniform float uGrayscale;
uniform vec3 uCameraPos;

in vec4 vCol;
in vec2 vUV;
in vec3 vNor;
in vec3 vPos;
in vec3 vNormalWorldSpace;

out vec4 fragColor;

uniform sampler2D tex;

void setBrightness(inout vec3 c, float brightness) {
    c = clamp(c + brightness, 0.0f, 1.0f);
}

void setContrast(inout vec3 c, float contrast) {
    float f = (contrast + 1.0f) / (1.0f - contrast);
    c = f * (c - 0.5f) + 0.5f;
}

void setGrayscale(inout vec3 c, float grayscale) {
    float f = (c.x + c.y + c.z) / 3;
    c = mix(vec3(f), c, grayscale);
}

vec3 ambientReflection (float intesity, float factor, vec3 lightColor) {
    return intesity * factor * lightColor;
}

vec3 diffuseReflection(float intensity, float factor, vec3 lightColor, vec3 lightDirection, vec3 normal) {
    return clamp(dot(lightDirection, normal), 0.0f, 1.0f) * intensity * factor * lightColor;
}

vec3 specularReflection(float intensity, float factor, vec3 lightColor, float hardness, vec3 viewDirection, vec3 reflextionDirection) {
    return pow(clamp(dot(viewDirection, reflextionDirection), 0.0f, 1.0f), hardness) * intensity * factor * lightColor;
}

vec3 specularBlinnReflection(float intensity, float factor, vec3 lightColor, float hardness, vec3 viewDirection, vec3 lightDirection, vec3 normal) {
    vec3 reflextionDirection = normalize(viewDirection + lightDirection);
    return pow(clamp(dot(normal, reflextionDirection), 0.0f, 1.0f), hardness) * intensity * factor * lightColor;
}

void main() {
    vec3 color = texture(tex, vUV).rgb;
    vec3 c = color * vCol.rgb;

    vec3 lightPosition = uLight[0].xyz;
    vec3 ambientLightColor = uLight[1].xyz;
    vec3 lightColor = uLight[2].xyz;

    float ambientIntensity = uLight[0].w;
    float diffuseIntensity = uLight[1].w;
    float specularIntensity = uLight[2].w;
    float hardness = uLight[3].w;

    float ambientFactor = uLight[3].x;
    float diffuseFactor = uLight[3].y;
    float specularFactor = uLight[3].z;

    vec3 lightDirection = normalize(lightPosition - vPos);

    vec3 normal = normalize(vNor);
    vec3 viewDirection = normalize(uCameraPos - vPos);
    vec3 reflectionDirection = reflect(-lightDirection, normal);

    vec3 ambient = ambientReflection(ambientIntensity, ambientFactor, ambientLightColor);
    vec3 diffuse = diffuseReflection(diffuseIntensity, diffuseFactor, lightColor, lightDirection, normal);
    // vec3 specular = specularReflection(specularIntensity, specularFactor, lightColor, hardness, viewDirection, reflectionDirection);
    vec3 specular = specularBlinnReflection(specularIntensity, specularFactor, lightColor, hardness, viewDirection, lightDirection, normal);

    vec3 finalColor = (ambient + diffuse + specular) * c;

    fragColor = vec4(finalColor, 1.0f);
}