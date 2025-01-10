shader_type canvas_item;

void vertex() {
	VERTEX.x += sin(1.0 * TIME) * pow((1.0 - UV.y), 3) * 8.0;
	VERTEX.y += sin(2.0 * TIME + PI/2.0) * pow((1.0 - UV.y), 1) * 1.0;
}

void fragment() {
	// Called for every pixel the material is visible on.
}

//void light() {
	// Called for every pixel for every light affecting the CanvasItem.
	// Uncomment to replace the default light processing function with this one.
//}
