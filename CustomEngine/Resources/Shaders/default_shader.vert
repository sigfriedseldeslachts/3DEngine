#version 330 core
layout (location = 0) in vec3 vPos;   // the position variable has attribute position 0
layout (location = 1) in vec3 vNormal;
layout (location = 2) in vec2 aTexCoord; // the texture variable has attribute position 2

out vec3 normal;
out vec3 FragPos;
out vec2 TexCoord;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;

void main()
{
    gl_Position = projection * view * model * vec4(vPos, 1.0);
    FragPos = vec3(model * vec4(vPos, 1.0));
    normal = mat3(transpose(inverse(model))) * vNormal;
    TexCoord = aTexCoord;
}