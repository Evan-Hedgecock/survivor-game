using Microsoft.Xna.Framework;

namespace Core.Systems;
public class TimerManager(Timer[] timers)
{
	private readonly Timer[] _timers = timers;

    public void Update(GameTime gameTime) {
		// Update every timer that is active
		foreach (Timer timer in _timers) {
			if (timer.IsActive()) {
				timer.CountDown(gameTime);
			}
		}
	}
}