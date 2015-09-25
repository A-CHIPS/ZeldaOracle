﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using XnaSong = Microsoft.Xna.Framework.Media.Song;
using XnaPlaylist = Microsoft.Xna.Framework.Media.Playlist;

using ZeldaOracle.Common.Audio;
using ZeldaOracle.Common.Graphics;
//using ZeldaOracle.Common.Graphics.Particles;
using ZeldaOracle.Common.Scripts;
using ZeldaOracle.Common.Translation;
using Song = ZeldaOracle.Common.Audio.Song;
using Playlist = ZeldaOracle.Common.Audio.Playlist;

namespace ZeldaOracle.Common.Content {

	public class LoadContentException : Exception {
		public LoadContentException(string message) :
			base(message) {
		}

		public virtual void PrintMessage() {
			Console.WriteLine(Message);
		}
	}

/** <summary>
 * The Resources class serves as a resource manager.
 * It has methods to load in resources from the game
 * content and stores them in key/value maps.
 * </summary> */
public class Resources {


	//========== CONSTANTS ===========
	#region Constants

	// Graphics
	/** <summary> The directory for storing images. </summary> */
	public const string ImageDirectory = "Images/";
	/** <summary> The directory for storing fonts. </summary> */
	public const string FontDirectory = "Fonts/";
	/** <summary> The directory for storing sprite sheets. </summary> */
	public const string SpriteSheetDirectory = "SpriteSheets/";
	/** <summary> The directory for storing animations. </summary> */
	public const string AnimationDirectory = "Animations/";
	/** <summary> The directory for storing shaders. </summary> */
	public const string ShaderDirectory = "Shaders/";

	// Sounds
	/** <summary> The directory for storing sounds. </summary> */
	public const string SoundDirectory = "Sounds/";
	/** <summary> The directory for storing songs. </summary> */
	public const string MusicDirectory = "Music/";

	// Languages
	/** <summary> The directory for storing languages. </summary> */
	public const string LanguageDirectory = "Languages/";

	#endregion
	//========== VARIABLES ===========
	#region Variables

	// Containment
	/** <summary> The game's content manager. </summary> */
	private static ContentManager contentManager;
	/** <summary> The game's graphics device. </summary> */
	private static GraphicsDevice graphicsDevice;

	// Graphics
	/** <summary> The collection of loaded images. </summary> */
	private static Dictionary<string, Image> images;
	/** <summary> The collection of loaded real fonts. </summary> */
	private static Dictionary<string, RealFont> realFonts;
	/** <summary> The collection of loaded game fonts. </summary> */
	private static Dictionary<string, GameFont> gameFonts;
	/** <summary> The collection of loaded sprite sheets. </summary> */
	private static Dictionary<string, SpriteSheet> spriteSheets;
	/** <summary> The collection of loaded sprites. </summary> */
	private static Dictionary<string, Sprite> sprites;
	/** <summary> The collection of loaded animations. </summary> */
	private static Dictionary<string, Animation> animations;
	/** <summary> The collection of loaded shaders. </summary> */
	private static Dictionary<string, Effect> shaders;
	/** <summary> The texture loader for loading images from file. </summary> */
	private static TextureLoader textureLoader;

	// Sounds
	/** <summary> The collection of loaded sound effects. </summary> */
	private static Dictionary<string, Sound> sounds;
	/** <summary> The collection of loaded songs. </summary> */
	private static Dictionary<string, Song> songs;
	/** <summary> The collection of loaded playlists. </summary> */
	private static Dictionary<string, Playlist> playlists;

	// Languages
	/** <summary> The collection of loaded languages. </summary> */
	private static List<Language> languages;

	// Settings
	/** <summary> True if the resource manager should output load information to the console. </summary> */
	private static bool verboseOutput;

	#endregion
	//========= CONSTRUCTORS =========
	#region Constructors

	/** <summary> Initializes the resource manager. </summary> */
	public static void Initialize(ContentManager contentManager, GraphicsDevice graphicsDevice) {
		// Containment
		Resources.contentManager	= contentManager;
		Resources.graphicsDevice	= graphicsDevice;

		// Graphics
		images				= new Dictionary<string, Image>();
		realFonts			= new Dictionary<string, RealFont>();
		gameFonts			= new Dictionary<string, GameFont>();
		spriteSheets		= new Dictionary<string, SpriteSheet>();
		sprites				= new Dictionary<string, Sprite>();
		animations			= new Dictionary<string, Animation>();
		shaders				= new Dictionary<string, Effect>();
		textureLoader		= new TextureLoader(graphicsDevice);

		// Sounds
		sounds				= new Dictionary<string, Sound>();
		songs				= new Dictionary<string, Song>();
		playlists			= new Dictionary<string, Playlist>();

		// Languages
		languages			= new List<Language>();

		// Settings
		verboseOutput		= false;
	}

	#endregion
	//========== RESOURCES ===========
	#region Resources
	//--------------------------------
	#region Graphics

	/** <summary> Gets the image with the specified name. </summary> */
	public static Image GetImage(string name) {
		return images[name];
	}
	/** <summary> Returns true if an image with the specified name exists. </summary> */
	public static bool ImageExists(string name) {
		return images.ContainsKey(name);
	}
	/** <summary> Gets the real font with the specified name. </summary> */
	public static RealFont GetRealFont(string name) {
		return realFonts[name];
	}
	/** <summary> Returns true if a real font with the specified name exists. </summary> */
	public static bool RealFontExists(string name) {
		return realFonts.ContainsKey(name);
	}
	/** <summary> Gets the game font with the specified name. </summary> */
	public static GameFont GetGameFont(string name) {
		return gameFonts[name];
	}
	/** <summary> Returns true if a game font with the specified name exists. </summary> */
	public static bool GameFontExists(string name) {
		return gameFonts.ContainsKey(name);
	}
	/** <summary> Gets the sprite sheet with the specified name. </summary> */
	public static SpriteSheet GetSpriteSheet(string name) {
		return spriteSheets[name];
	}
	/** <summary> Returns true if a sprite sheet with the specified name exists. </summary> */
	public static bool SpriteSheetExists(string name) {
		return spriteSheets.ContainsKey(name);
	}
	/** <summary> Gets the sprite with the specified name. </summary> */
	public static Sprite GetSprite(string name) {
		return sprites[name];
	}
	/** <summary> Returns true if a sprite with the specified name exists. </summary> */
	public static bool SpriteExists(string name) {
		return sprites.ContainsKey(name);
	}
	/** <summary> Gets the animation with the specified name. </summary> */
	public static Animation GetAnimation(string name) {
		return animations[name];
	}
	/** <summary> Returns true if an animation with the specified name exists. </summary> */
	public static bool AnimationExists(string name) {
		return animations.ContainsKey(name);
	}
	/** <summary> Gets the shader with the specified name. </summary> */
	public static Effect GetShader(string name) {
		return shaders[name];
	}
	/** <summary> Returns true if a shader with the specified name exists. </summary> */
	public static bool ShaderExists(string name) {
		return shaders.ContainsKey(name);
	}

	#endregion
	//--------------------------------
	#region Sounds

	/** <summary> Gets the sound with the specified name. </summary> */
	public static Sound GetSound(string name) {
		return sounds[name];
	}
	/** <summary> Gets the song with the specified name. </summary> */
	public static Song GetSong(string name) {
		return songs[name];
	}
	/** <summary> Gets the playlist with the specified name. </summary> */
	public static Playlist GetPlaylist(string name) {
		return playlists[name];
	}

	#endregion
	//--------------------------------
	#region Languages

	/** <summary> Gets the language with the specified name. </summary> */
	public static Language GetLanguage(string name) {
		foreach (Language language in languages) {
			if (language.Name == name)
				return language;
		}
		return null;
	}

	#endregion
	//--------------------------------
	#endregion
	//=========== LOADING ============
	#region Loading
	//--------------------------------
	#region Graphics

	/** <summary> Loads the image with the specified asset name. </summary> */
	public static Image LoadImage(string assetName) {
		string name = assetName.Substring(assetName.IndexOf('/') + 1);
		Image resource = new Image(contentManager.Load<Texture2D>(assetName), name);
		images.Add(name, resource);
		return resource;
	}
	/** <summary> Loads the image with the specified asset name. </summary> */
	public static Image LoadImageFromFile(string assetName) {
		string name = assetName.Substring(assetName.IndexOf('/') + 1);
		name = name.Substring(0, name.LastIndexOf('.'));
		Image resource = new Image(textureLoader.FromFile(contentManager.RootDirectory + "/" + assetName), name);
		images.Add(name, resource);
		return resource;
	}
	/** <summary> Loads the real font with the specified asset name. </summary> */
	public static RealFont LoadRealFont(string assetName) {
		string name = assetName.Substring(assetName.IndexOf('/') + 1);
		RealFont resource = new RealFont(contentManager.Load<SpriteFont>(assetName), name);
		realFonts.Add(name, resource);
		return resource;
	}
	/** <summary> Loads a shader (Effect). </summary> */
	public static Effect LoadShader(string assetName) {
		string name = assetName.Substring(assetName.IndexOf('/') + 1);
		Effect resource = contentManager.Load<Effect>(assetName);
		shaders.Add(name, resource);
		return resource;
	}
	/** <summary> Loads a single sprite sheet from a script file. </summary> */
	public static SpriteSheet LoadSpriteSheet(string assetName) {
		SpriteSheetSROld script = new SpriteSheetSROld();
		LoadScript(assetName, script);
		return script.Sheet;
	}
	/** <summary> Loads/compiles sprite sheets from a script file. </summary> */
	public static void LoadSpriteSheets(string assetName) {
		LoadScript(assetName, new SpritesSR());
	}
	/** <summary> Loads the game font with the specified asset name. </summary> */
	public static GameFont LoadGameFont(string assetName) {
		GameFontSR script = new GameFontSR();
		LoadScript(assetName, script);
		return script.Font;
	}
	/** <summary> Loads/compiles game fonts from a script file. </summary> */
	public static void LoadGameFonts(string assetName) {
		LoadScript(assetName, new GameFontSR());
	}
	/** <summary> Loads/compiles animations from a script file. </summary> */
	public static void LoadAnimations(string assetName) {
		LoadScript(assetName, new AnimationSR());
	}

	#endregion
	//--------------------------------
	#region Sounds

	/** <summary> Loads a sound effect. </summary> */
	public static Sound LoadSound(string assetName) {
		string name = assetName.Substring(assetName.IndexOf('/') + 1);
		Sound resource = new Sound(contentManager.Load<SoundEffect>(assetName), assetName, name);
		sounds.Add(name, resource);
		return resource;
	}
	/** <summary> Loads a song. </summary> */
	public static Song LoadSong(string assetName) {
		string name = assetName.Substring(assetName.IndexOf('/') + 1);
		Song resource = new Song(contentManager.Load<SoundEffect>(assetName), assetName, name);
		songs.Add(name, resource);
		return resource;
	}
	/** <summary> Loads a sounds file. </summary> */
	public static void LoadSounds(string assetName) {
		LoadScript(assetName, new SoundsSR());
	}
	/** <summary> Loads a music file. </summary> */
	public static void LoadMusic(string assetName) {
		LoadScript(assetName, new MusicSR());
	}

	#endregion
	//--------------------------------
	#region Languages

	/** <summary> Loads a language file. </summary> */
	public static Language LoadLanguage(string assetName) {
		assetName = assetName.Substring(assetName.IndexOf('/') + 1);
		LanguageSR script = new LanguageSR();
		LoadScript(assetName, script, Encoding.UTF8);
		return script.Language;
	}

	#endregion
	//--------------------------------
	#endregion
	//=========== SCRIPTS ============
	#region Scripts

	/** <summary> Loads a script file with the given script reader object. </summary> */
	public static void LoadScript(string assetName, ScriptReader sr) {
		LoadScript(assetName, sr, Encoding.Default);
	}
	/** <summary> Loads a script file with the given encoding and script reader object. </summary> */
	public static void LoadScript(string assetName, ScriptReader sr, Encoding encoding) {
		try {
			Stream stream = TitleContainer.OpenStream(contentManager.RootDirectory + "/" + assetName);
			StreamReader reader = new StreamReader(stream, encoding);
			sr.ReadScript(reader);
			stream.Close();
		}
		catch (FileNotFoundException) {
			Console.WriteLine("Error loading file \"" + assetName + "\"");
		}
	}

	#endregion
	//============ ADDING ============
	#region Adding
	//--------------------------------
	#region Graphics

	/** <summary> Adds the specified sprite sheet. </summary> */
	public static void AddSpriteSheet(string assetName, SpriteSheet sheet) {
		spriteSheets.Add(assetName, sheet);
	}
	/** <summary> Adds the specified sprite. </summary> */
	public static void AddSprite(string assetName, Sprite sprite) {
		sprites.Add(assetName, sprite);
	}
	/** <summary> Adds the specified game font. </summary> */
	public static void AddGameFont(string assetName, GameFont font) {
		gameFonts.Add(assetName, font);
	}
	/** <summary> Adds the specified animation. </summary> */
	public static void AddAnimation(string assetName, Animation animation) {
		animations.Add(assetName, animation);
	}

	#endregion
	//--------------------------------
	#region Sounds

	#endregion
	//--------------------------------
	#region Languages

	/** <summary> Adds the specified language. </summary> */
	public static void AddLanguage(Language language) {
		languages.Add(language);
	}

	#endregion
	//--------------------------------
	#endregion
	//========== PROPERTIES ==========
	#region Properties

	/** <summary> Gets or sets if the resource manager should output load information to the console. </summary> */
	public static bool VerboseOutput {
		get { return verboseOutput; }
		set { verboseOutput = value; }
	}
	/** <summary> Gets the dictionary of sounds. </summary> */
	public static Dictionary<string, Sound> Sounds {
		get { return sounds; }
	}
	/** <summary> Gets the list of langauges. </summary> */
	public static List<Language> Languages {
		get { return languages; }
	}
	/** <summary> Gets the list of sprite sheets. </summary> */
	public static SpriteSheet[] SpriteSheets {
		get {
			SpriteSheet[] sheets = new SpriteSheet[spriteSheets.Count];
			spriteSheets.Values.CopyTo(sheets, 0);
			return sheets;
		}
	}
	/** <summary> Gets the number of sprite sheets. </summary> */
	public static int SpriteSheetCount {
		get { return spriteSheets.Count; }
	}

	#endregion
}
} // end namespace
