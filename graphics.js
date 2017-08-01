/****************************************************/
/* graphics.js			     		    */
/*						    */
/* This file contains a collection of graphical	    */
/* classes and functions which can be used to draw  */
/* on to a HTML5 canvas.			    */
/*						    */
/* Uses: core.js				    */
/*						    */
/*						    */
/*                                George McDonagh   */
/*						    */
/****************************************************/

///////////////////////////////////////
// CAMERA /////////////////////////////
///////////////////////////////////////
function Camera() {
	this.tX = 0;
	this.tY = 0;
	this.sX = 1;
	this.sY = 1;
}
Camera.prototype.setFocus = function(focusPoint) {
	this.tX = Math.round((canvas.width / 2) - focusPoint.x);
	this.tY = Math.round((canvas.height / 2) - focusPoint.y);
};

///////////////////////////////////////
// TEXTURE ////////////////////////////
///////////////////////////////////////

/**
* Texture
* Wraps an Image object and keeps its width and height
* @param filePath : The location and name of the image file
* @param width    : The width of the image file in pixels
* @param height   : The height of the image file in pixels
*/
function Texture(filePath, width, height) {
	if (filePath !== null) {
		this.image = new Image();
		this.image.src = filePath;
	}

	this.width = width;
	this.height = height;
}

///////////////////////////////////////
// SPRITE /////////////////////////////
///////////////////////////////////////

/**
* Sprite
* A drawable Texture which can be scaled and clipped
* @param texture : The Texture object that the Sprite will draw
* @param x       : X pixel coordinate
* @param y       : Y pixel coordinate
* @param width   : The width the texture will be drawn to
* @param height  : The height the texture will be drawn to
*/
function Sprite(texture, x, y, width, height) {
	this.texture = texture;
	this.source = new Rect(0, 0, this.texture.width, this.texture.height);

	// Call the parent classes constructor
	Rect.call(this, x, y, width, height);
}
Sprite.prototype = Object.create(Rect.prototype);	// Inherit from Rect
Sprite.prototype.constructor = Sprite;				// Make sure constructor points to Sprite's constructor
/**
* This function that draws to the contexts
*/
Sprite.prototype.draw = function(context) {
	context.drawImage(this.texture.image, 
		this.source.x, this.source.y, this.source.width, this.source.height, // Souce rectangle
		this.x, this.y, this.width, this.height); // Destination rectangle
};

///////////////////////////////////////
// ANIMATED SPRITE ////////////////////
///////////////////////////////////////

/**
* Animated Sprite
* A child of Sprite that can animate from a sprite sheet
* @param duration   : The duration in seconds one loop of the animation will run for
* @param frameWidth : The width in pixels of one frame on the sprite sheet
*/
function AnimatedSprite(texture, x, y, width, height, duration, frameWidth) {
	this.duration = duration; 
	this.elapsed = 0;			// The elapsed time since the beginning of the current animation loop
	this.currentFrame = 0;		// The index of the current frame's source rectangle
	this.playing = false;		// The animation will run when true
	this.sources = [];			// A collection of the source rectangles for the sprite sheet

	// Fill the list of source rectangles
	for (var i = 0; i < texture.width / frameWidth; i++) {
		this.sources.push(new Rect(i * frameWidth, 0, frameWidth, texture.height));
	}

	// Call parent's constructor
	Sprite.call(this, texture, x, y, width, height);

	// Set the starting source rect to the first one in the list
	this.source = this.sources[0];
}
AnimatedSprite.prototype = Object.create(Sprite.prototype); // Inherit from Sprite
AnimatedSprite.prototype.constructor = AnimatedSprite;
/**
* This function updates the animation
*/
AnimatedSprite.prototype.update = function(dt) {
	// Only update if we're supposed to
	if (this.playing) {
		// Increment elapsed time
		this.elapsed += dt;

		if (this.elapsed > this.duration / this.sources.length) {
			// Reset elapsed time if we went over the allocated time for this frame 
			this.elapsed = 0;

			// If the current frame is not the last one, then we increment it, otherwise it goes back to the start
			this.currentFrame = (this.currentFrame < this.sources.length - 1) ? this.currentFrame + 1 : 0;

			// Update the source rectangle
			this.source = this.sources[this.currentFrame];
		}
	}
};
/**
* Stopping function that resets the elapsed time, current frame, playing boolean, and source rect.
*/
AnimatedSprite.prototype.stop = function() {
	this.elapsed = 0;
	this.currentFrame = 0;	
	this.playing = false;
	this.source = this.sources[0];
};

///////////////////////////////////////
// TEXT ///////////////////////////////
///////////////////////////////////////

/**
* Text
*
*/
function Text(text, position) {
	this.text = text;
	this.position = position;
	this.font = "12pt Consolas";
	this.fillStyle = "white";
	this.textAlign = "Left";
}
Text.prototype.getSize = function() {
	context.font = this.font;
	return new Vec2(context.measureText(this.text).width, context.measureText(this.text).height);
};
Text.prototype.draw = function(context) {
	context.font = this.font;
	context.fillStyle = this.fillStyle;
	context.textAlign = this.textAlign;
	context.fillText(this.text, this.position.x, this.position.y);
};
