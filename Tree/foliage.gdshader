shader_type canvas_item;
render_mode blend_mix;

uniform sampler2D shader_mask : source_color ,repeat_enable;


void fragment() {
	//vec4 mask = texture(shader_mask, UV);
	////COLOR = texture(TEXTURE, UV);
	//if (mask.a > 0.0){
		//COLOR.b += 0.4;
	//}
}

// Helper function to generate pseudo-random values
float rand(vec2 co) {
    return fract(sin(dot(co.xy, vec2(12.9898, 78.233))) * 43758.5453);
}

float leafT(vec2 _UV) {
	// Time-based wind oscillation
    
	float windStrength = 0.2; // Adjust for overall sway intensity
    float windFrequency = 5.2; // General wind frequency
    float gustFrequency = 0.0; // Gust-like variations

    // Generate pseudo-random offsets based on UV coordinates
    float randomOffset = rand(_UV * 10.0);

    // Time-based wind oscillation with pseudo-randomness
    float baseWind = sin(windFrequency * TIME + randomOffset * 6.2831) * windStrength;
    float gustWind = cos(gustFrequency * TIME + randomOffset * 12.5663) * (windStrength * 0.3);

    // Combine base wind and gust wind
    float t = baseWind + gustWind;
	return t;
}

void vertex() {
	float sway = sin(TIME);
	VERTEX.x += sway * (1.0 - UV.y) * 0.05;
	

	if (texture(shader_mask, UV).a == 1.0) {
        VERTEX += leafT(UV);
    }
	
}

