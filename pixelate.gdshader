shader_type canvas_item;

uniform float pixel_size : hint_range(1.0, 50.0) = 8.0;


uniform sampler2D screen_texture : hint_screen_texture, repeat_disable, filter_nearest;

void fragment() {
    // Get the screen texture UV
    vec2 uv = SCREEN_UV;

    
    COLOR = texture(screen_texture, pixelated_uv);
}