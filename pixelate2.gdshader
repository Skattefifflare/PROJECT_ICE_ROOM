shader_type canvas_item;

uniform float pixel_size : hint_range(1.0, 50.0) = 8.0;


uniform sampler2D screen_texture : hint_screen_texture, repeat_disable, filter_nearest;

void fragment() {
    // Convert UV to absolute screen-space coordinates
    vec2 screen_pos = UV.xy;

    // Apply pixelation in absolute screen-space
    vec2 pixelated_pos = floor(screen_pos / pixel_size) * pixel_size;

    // Convert back to UV space
    vec2 pixelated_uv = pixelated_pos / vec2(SCREEN_PIXEL_SIZE.x, SCREEN_PIXEL_SIZE.y);

    // Sample from the screen texture
    COLOR = texture(screen_texture, pixelated_uv);
}

