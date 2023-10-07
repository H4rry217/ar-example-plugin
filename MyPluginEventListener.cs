using AimRobot.Api;
using AimRobot.Api.events;
using AimRobot.Api.events.ev;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ar_example_plugin {
    public class MyPluginEventListener : IEventListener{

        [AimRobot.Api.events.EventHandler]
        public void whenPlayerChat(PlayerChatEvent playerChatEvent) {
            if (playerChatEvent.message.Contains("shabi")) {
                Robot.GetInstance().BanPlayer(playerChatEvent.speaker, "说脏话");
            }
        }

    }
}
