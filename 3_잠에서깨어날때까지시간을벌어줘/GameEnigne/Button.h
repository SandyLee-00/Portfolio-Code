#pragma once
#include "Object.h"
#include "Vector2.h"

/// <summary>
/// ¿Ãº≠øµ
///
/// </summary>

class Button 
	: public Object
{
protected:
// 	Sprite* sourceImage;
// 	Sprite* pressedImage;
	Vector2 size;
	std::string text;
	Vector2 stringPosition;
	float fontSize = 20.0f;

private:
	int cnt;

public:
	Button(ManagerSet* _manager);
	virtual ~Button();

	virtual void Start() override;
	virtual void Update(float _deltaTime) override;
	virtual void FinalUpdate(float _deltaTime) override;
	virtual void Render(GraphicsEngine* _graphicsEngine) override;
	virtual void DebugRender(GraphicsEngine* _graphicsEngine) override;

	void SetSize(Vector2 _size) { size = _size; }
	void SetTextAll(std::string _text, Vector2 _position, float _size) { text = _text; stringPosition = _position; fontSize = _size; }

	void SetTextString(std::string _text) { text = _text; }
	void SetTextPosition(Vector2 _position) { stringPosition = _position; }
	void SetTextFontSize(float _size) { fontSize = _size; }

	virtual void OnButtonClicked();


};

