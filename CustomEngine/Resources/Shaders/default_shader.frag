#version 330 core
in vec2 TexCoord;

uniform sampler2D texture_diffuse0;

out vec4 FragColor;

void main()
{
    FragColor = texture(texture_diffuse0, TexCoord);
}