# PROJECT_ICE_ROOM
The most ambitious game made since Skyrim.



## FORMATERINGSGUIDE
### Mappordning i Godot
Mappar och filer sorteras i detta mönster:<br/>
res://
  - Creatures
	- Cow
	  - CowObj.tscn
	  - CowScript.cs
	  - CowSprite.png
  - Weapons
	- Spear
	  - SpearObj.tscn
	  - SpearSprite.png
	
osv.

### Namngivning
> [!WARNING]
> OTROLIGT UP FOR DEBATE

| Fil-/Objekttyp| Namnformat     |
| ------------- | -------------- |
| mappar        | ALL_CAPS       |
| noder         | PascalCase     |
| .cs-scripts   | PascalCase     |
| .gd-scripts   | PascalCaseGD   |
| medlemsvars   | ALL_CAPS       |
| vars          | all_smalls     |
| metoder       | PascalCase     |


### Kommentarer

Alla kommentarer skrivs med **//**<br/>
/* */ används för utkommentering av kodblock<br/>
utanför utkommenterade kodblock ska en motivation stå om varför kodblocket ska sparas<br/>
exempel:<br/>
```c#
// kanske behöver detta sen
/*
if (true){
	bool notTrue = false
}
*/
```




## DOKUMENTATION AV GITHUB OCH GODOT
### Godot-versionen är 4.3 .NET<br/>
Ladda ner [här](https://godotengine.org/releases/4.3/)

### För att hämta repot:<br/>
I Github Desktop, klicka **File->Clone Repository**

### För att visa procedurell generering i editorn:<br/>
Skriv **[Tool]** i nodens script på raden ovanför **public partial class**<br/>
I VS, klicka **Build-> Build Solution**<br/>
ytterligare info [här](https://docs.godotengine.org/en/stable/tutorials/plugins/running_code_in_the_editor.html)

### Egna properties för noder:<br/>
använd **[Export]** på raden ovanför en nods medlemsvariabel för att göra den ändringsbar i editorn<br/>
mer info [här](https://docs.godotengine.org/en/stable/tutorials/scripting/c_sharp/c_sharp_exports.html)
  
<!--För att styla README-filen kolla här, https://docs.github.com/en/get-started/writing-on-github/getting-started-with-writing-and-formatting-on-github/basic-writing-and-formatting-syntax -->
