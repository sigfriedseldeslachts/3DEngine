#version 330 core
in vec2 TexCoord;

//uniform sampler2D uTexture0;

out vec4 FragColor;

void main()
{
    FragColor = vec4(1.0f, 0.5f, 0.2f, 1.0f); //texture(uTexture0, TexCoord);
}