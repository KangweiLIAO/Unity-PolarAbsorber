using Platformer.Core;
using Platformer.Mechanics;
using Platformer.Model;
using UnityEngine;
using static Platformer.Core.Simulation;

namespace Platformer.Gameplay
{

    /// <summary>
    /// Fired when a Player collides with an Enemy.
    /// </summary>
    /// <typeparam name="EnemyCollision"></typeparam>
    public class PlayerEnemyCollision : Simulation.Event<PlayerEnemyCollision>
    {
        public EnemyController enemy;
        public PlayerController player;
 

        PlatformerModel model = Simulation.GetModel<PlatformerModel>();

        public override void Execute()
        {
                var enemyHealth = enemy.GetComponent<Health>();
                if (enemyHealth != null)
                {
                  Schedule<EnemyDeath>().enemy = enemy;
                }
                else
                {
                    Schedule<EnemyDeath>().enemy = enemy;
                    player.Bounce(2);
                    if (GameController.powerTimer > 0) {
                        GameController.ReduceTimer(-20);
                    } else {
                    GameController.ReduceTimer(20);
                    }
                }
        }
    }
}