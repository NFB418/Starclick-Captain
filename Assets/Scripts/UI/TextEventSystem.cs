using System.Text;
using UnityEngine;

public class TextEventSystem : MonoBehaviour
{
    public static TextEventSystem instance;
    private void Awake() => instance = this;

    public TextEventWindow textEventWindow;

    private void StartTextEvent(string content, string header = "")
    {
        textEventWindow.SetText(content, header);
        textEventWindow.OpenWindow();
    }

    public void NewGameEvent()
    {
        string header = "A New Beginning";

        StringBuilder builder = new();
        builder.Append("<i>");
        builder.Append(
            "Congratulations, Captain, on your first command! A shame it had to happen under such dire circumstances, but the admiralty is confident you'll make Earth proud. " +
            "Now, we're quite short on personnel at the moment, so your first mission is to get out there and scrounge up some crew! " +
            "Don't worry, your ship is fully automatic and will generate power at the press of a button."
            );
        builder.Append("</i>");
        builder.AppendLine(); builder.AppendLine();
        builder.Append(
            "Welcome to the Starclick Captain prototype. Before we start, please be advised that almost everything in this game comes with a tooltip explaining what it means. " +
            "Just hover your cursor over any part of the interface to learn more about it."
            );
        builder.AppendLine(); builder.AppendLine();
        builder.Append(
            "Your goal in this game is to gather <b>power</b>. Almost anything can be a source of <b>power</b>: " +
            "politics, science, spirituality... All forms of <b>power</b> can be collected and will be represented as a single value in the top left of the screen."
            );
        builder.AppendLine(); builder.AppendLine();
        builder.Append(
            "For now, start by <b>clicking</b> the <b>ship</b> a bunch of times to generate your first <b>power</b>!"
            );

        string content = builder.ToString();

        this.StartTextEvent(content, header);
    }

    public void FirstEnsignEvent()
    {
        string header = "Your First Ensign";

        StringBuilder builder = new();
        builder.Append("<i>");
        builder.Append(
            "Good job getting your first crew member! Now, Captain, your mission is simple: we need power, and lots of it. " +
            "We don't care what kind of power or how you get it, just take whatever you can find and bring it back to HQ! " +
            "If you do well, there's a promotion to vice-admiral with your name on it...."
            );
        builder.Append("</i>");
        builder.AppendLine(); builder.AppendLine();
        builder.Append(
            "Now that you've recruited your first ensign, let's explain this game's two most important mechanics: <b>click power</b> and <b>click limit</b>."
            );
        builder.AppendLine(); builder.AppendLine();
        builder.Append(
            "<b>Click power</b> is the amount of <b>power</b> you gain for every <b>click</b>. " +
            "This applies not only to your own <b>clicks</b> on the <b>ship</b>, but to everything the game calls a <b>click</b>. " +
            "For example, your ensigns will also produce your <b>click power</b>'s worth for each <b>click</b> they produce."
            );
        builder.AppendLine(); builder.AppendLine();
        builder.Append(
            "The <b>click limit</b> determines how many times you can <b>click</b> the <b>ship</b> per second. " +
            "Most sources of power that are not directly tied to <b>clicking</b> will instead give you an amount of power based on your <b>click power</b> multiplied by your <b>click limit</b>."
            );
        builder.AppendLine(); builder.AppendLine();
        builder.Append(
            "Now, you can either keep <b>clicking</b> away or, if you want, sit back and let your ensigns do the <b>clicking</b> for you until your next unlock!"
            );

        string content = builder.ToString();

        this.StartTextEvent(content, header);
    }

    public void FirstEngineerEvent()
    {
        string header = "Your First Engineer";

        StringBuilder builder = new();
        builder.Append("<i>");
        builder.Append(
            "An engineer never fixes the ship too late, nor too early. Engineers fix the ship in the nick of time, precisely as they're meant to."
            );
        builder.Append("</i>");
        builder.AppendLine(); builder.AppendLine();
        builder.Append(
            "You've unlocked engineers! Each engineer will increase your <b>click power</b>. " +
            "As we've discussed before, <b>click power</b> applies to all <b>clicks</b>, not just your own. " +
            "So each engineer will also increase the power output of every one of your ensigns."
            );
        builder.AppendLine(); builder.AppendLine();
        builder.Append(
            "Good job getting this far. Keep gathering more power to unlock another crew type!"
            );

        string content = builder.ToString();

        this.StartTextEvent(content, header);
    }

    public void FirstAnomalyEvent()
    {
        string header = "Your First Anomaly";

        StringBuilder builder = new();
        builder.Append("<i>");
        builder.Append(
            "Fascinating, Captain. This anomaly seems to have been filled with caffeine. If we could harness the quantum processes responsible... the possibilities are limitless."
            );
        builder.Append("</i>");
        builder.AppendLine(); builder.AppendLine();
        builder.Append(
            "You've scanned an <b>anomaly</b>! <b>Anomalies</b> will keep appearing at random. Scan them to gain <b>power</b> based on your <b>click power</b> multiplied by your <b>click limit</b>."
            );

        string content = builder.ToString();

        this.StartTextEvent(content, header);
    }

    public void FirstDoctorEvent()
    {
        string header = "Your First Doctor";

        StringBuilder builder = new();
        builder.Append("<i>");
        builder.Append(
            "You never need a doctor, until you do, and then you're glad you hired a doctor and not another ensign or engineer!"
            );
        builder.Append("</i>");
        builder.AppendLine(); builder.AppendLine();
        builder.Append(
            "You've unlocked doctors! Each doctor will lower the cost of hiring more crew by changing the hirin cost to what it would be " +
            $"if you had {CrewManager.instance.crewDoctorReductionMult} less crew of that type (to a minimum of 0). "
            );

        string content = builder.ToString();

        this.StartTextEvent(content, header);
    }

    public void FirstCaptainsClickEvent()
    {
        string header = "Your Captain's Click";

        StringBuilder builder = new();
        builder.Append("<i>");
        builder.Append(
            "Captain, you really shouldn't be doing so much clicking... that's what the ensigns are for!"
            );
        builder.Append("</i>");
        builder.AppendLine(); builder.AppendLine();
        builder.Append(
            "Did you notice something strange about that last <b>click</b>? You have now unlocked the <b>captain's click</b> ability! " +
            "Every second that you do NOT click the ship, your <b>captain's click</b> charges two things:"
            );
        builder.AppendLine(); builder.AppendLine();
        builder.Append(
            "1) It will store your <b>click limit</b> in <b>clicks</b> (up to the specified limit)."
            );
        builder.AppendLine(); builder.AppendLine();
        builder.Append(
            "2) It will also charge your <b>captain's click charge</b> (also up to its specified limit)."
            );
        builder.AppendLine(); builder.AppendLine();
        builder.Append(
            "Whenever you DO <b>click</b> the <b>ship</b>, you will be spending everything you've save up according to the following formula: " +
            "your <b>click power</b> multiplied by the number of <b>clicks</b> stored multiplied by your <b>captain's click charge</b>."
            );
        builder.AppendLine(); builder.AppendLine();
        builder.Append(
            "So, save those fingers and only <b>click</b> the <b>ship</b> when you really want to!"
            );

        string content = builder.ToString();

        this.StartTextEvent(content, header);
    }

    public void FirstScientistEvent()
    {
        string header = "Your First Scientist";

        StringBuilder builder = new();
        builder.Append("<i>");
        builder.Append(
            "Scientists are always getting starships into trouble. Thankfully they usually techno-babble it out of trouble too, eventually..."
            );
        builder.Append("</i>");
        builder.AppendLine(); builder.AppendLine();
        builder.Append(
            "Well done, this will be the last tutorial screen you see! You have unlocked scientists. Each scientist increases your <b>antimatter generation</b> and <b>antimatter storage</b>."
            );
        builder.AppendLine(); builder.AppendLine();
        builder.Append(
            "<b>Antimatter</b> is used to power the abilities of your bridge officers. Some bridge officers consume <b>antimatter</b> directly each time you activate their ability. " +
            "Other bridge officers passively lock a percentage of your <b>antimatter storage</b>, making it unavailable to other officers, to power their passive abilities. " +
            "(Such abilities will not activate until you have accumulated <b>antimatter</b> equal to the lock percentage.)"
            );
        builder.AppendLine(); builder.AppendLine();
        builder.Append(
            "You should keep hiring more scientists to unlock more bridge officers, and experiment with their abilities to help you accumulate enough <b>power</b> to complete this demo! Good luck!"
            );

        string content = builder.ToString();

        this.StartTextEvent(content, header);
    }
}
