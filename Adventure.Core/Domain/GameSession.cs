using Adventure.Core.Resources;
using System;
using System.Collections.Generic;

namespace Adventure.Core.Domain
{
    public class GameSession
    {
        public string Id { get; init; }

        public Player Player { get; }

        public Scene ActiveScene { get; private set; }

        private List<Scene> _scenes;


        public delegate void GameEventHandler<T>(GameSession gameSession, T arg);

        public event GameEventHandler<Scene> SceneChanged; 


        public GameSession()
        {
            Player = new Player();

            // Default scenes
            _scenes = new List<Scene>
            {
                // Forest
                new(SceneResources.ForestId, SceneResources.ForestDescription, new []
                {
                    new Action("gehe", "links", "rechts")
                })
            };
        }

        public void Start()
        {
            EnterScene(SceneResources.ForestId);
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
