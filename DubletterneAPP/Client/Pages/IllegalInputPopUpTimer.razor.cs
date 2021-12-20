namespace DubletterneAPP.Client;

public partial class IlligalInputPopUpTimer{

    private DateTime time;

    public IlligalInputPopUpTimer(){
        time = DateTime.Now;
    }

    public bool popUpProvoked(DateTime currentTime){
        var timeSpan = currentTime - time;
        time = currentTime;
        return timeSpan.TotalSeconds > 1 ? true : false;
    }
}