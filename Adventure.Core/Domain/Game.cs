using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adventure.Core.Domain
{
    public class Game
    {
        public string Id { get; set; }

        public Player Player { get; }

        public Scene ActiveScene { get; private set; }

        private List<Scene> _scenes;


        public delegate void GameEventHandler<T>(Game game, T arg);

        public event GameEventHandler<Scene> SceneChanged; 


        public Game()
        {
            Player = new Player();

            // Scenes
            _scenes = new List<Scene>
            {
                new(SceneDefaults.Forest, "test")
            };
        }

        public void Start()
        {
            EnterScene(SceneDefaults.Forest);
        }

        private Scene EnterScene(string id)
        {
            if (!_scenes.Exists(x => x.Id == id))
                throw new Exception("Scene does not exist");

            ActiveScene = _scenes.Find(x => x.Id == id);
            SceneChanged?.Invoke(this, ActiveScene);

            return ActiveScene;
        }
    }
}
