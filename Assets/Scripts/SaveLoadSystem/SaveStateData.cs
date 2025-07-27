using System;

[Serializable]
public class SaveStateData
{
    // This class is meant to handle saving and loading of more complicated information, i.e. the state of dynamic UI elements, than what's done by the Data class.
    // Create a new instance of this class in Data for every save, and LoadIntoUI after every load.
    
    private char selectedCaptainPortrait;
    private string captainName;

    private SpaceCollectibleSaveData northSCSaveData;
    private SpaceCollectibleSaveData northeastSCSaveData;
    private SpaceCollectibleSaveData eastSCSaveData;
    private SpaceCollectibleSaveData southeastSCSaveData;
    private SpaceCollectibleSaveData southSCSaveData;
    private SpaceCollectibleSaveData southwestSCSaveData;
    private SpaceCollectibleSaveData westSCSaveData;
    private SpaceCollectibleSaveData northwestSCSaveData;

    private BridgeOfficerSaveData BOfc_0_SaveData;
    private BridgeOfficerSaveData BOfc_1_SaveData;
    private BridgeOfficerSaveData BOfc_2_SaveData;
    private BridgeOfficerSaveData BOfc_3_SaveData;

    public SaveStateData(Controller controller, BridgeOfcManager bridgeOfcManager)
    {
        selectedCaptainPortrait = bridgeOfcManager.captainPortraitSelectWindow.selectedCaptainPortrait;
        captainName = bridgeOfcManager.CaptainNameInput.text;

        SpaceCollectibleHandler northSpaceCollectible = Controller.instance.northSpaceCollectible;
        SpaceCollectibleHandler northeastSpaceCollectible = Controller.instance.northeastSpaceCollectible;
        SpaceCollectibleHandler eastSpaceCollectible = Controller.instance.eastSpaceCollectible;
        SpaceCollectibleHandler southeastSpaceCollectible = Controller.instance.southeastSpaceCollectible;
        SpaceCollectibleHandler southSpaceCollectible = Controller.instance.southSpaceCollectible;
        SpaceCollectibleHandler southwestSpaceCollectible = Controller.instance.southwestSpaceCollectible;
        SpaceCollectibleHandler westSpaceCollectible = Controller.instance.westSpaceCollectible;
        SpaceCollectibleHandler northwestSpaceCollectible = Controller.instance.northwestSpaceCollectible;

        northSCSaveData = new SpaceCollectibleSaveData(northSpaceCollectible);
        northeastSCSaveData = new SpaceCollectibleSaveData(northeastSpaceCollectible);
        eastSCSaveData = new SpaceCollectibleSaveData(eastSpaceCollectible);
        southeastSCSaveData = new SpaceCollectibleSaveData(southeastSpaceCollectible);
        southSCSaveData = new SpaceCollectibleSaveData(southSpaceCollectible);
        southwestSCSaveData = new SpaceCollectibleSaveData(southwestSpaceCollectible);
        westSCSaveData = new SpaceCollectibleSaveData(westSpaceCollectible);
        northwestSCSaveData = new SpaceCollectibleSaveData(northwestSpaceCollectible);

        BridgeOfcSlot bridgeOfcSlot_0 = bridgeOfcManager.bridgeOfcSlot_0;
        BridgeOfcSlot bridgeOfcSlot_1 = bridgeOfcManager.bridgeOfcSlot_1;
        BridgeOfcSlot bridgeOfcSlot_2 = bridgeOfcManager.bridgeOfcSlot_2;
        BridgeOfcSlot bridgeOfcSlot_3 = bridgeOfcManager.bridgeOfcSlot_3;

        BOfc_0_SaveData = new BridgeOfficerSaveData(bridgeOfcSlot_0.SlottedBridgeOfficer);
        BOfc_1_SaveData = new BridgeOfficerSaveData(bridgeOfcSlot_1.SlottedBridgeOfficer);
        BOfc_2_SaveData = new BridgeOfficerSaveData(bridgeOfcSlot_2.SlottedBridgeOfficer);
        BOfc_3_SaveData = new BridgeOfficerSaveData(bridgeOfcSlot_3.SlottedBridgeOfficer);
    }

    public void LoadIntoUI(Controller controller, BridgeOfcManager bridgeOfcManager) 
    {
        bridgeOfcManager.captainPortraitSelectWindow.SetPortrait(selectedCaptainPortrait);
        bridgeOfcManager.CaptainNameInput.text = captainName;

        SpaceCollectibleHandler northSpaceCollectible = Controller.instance.northSpaceCollectible;
        SpaceCollectibleHandler northeastSpaceCollectible = Controller.instance.northeastSpaceCollectible;
        SpaceCollectibleHandler eastSpaceCollectible = Controller.instance.eastSpaceCollectible;
        SpaceCollectibleHandler southeastSpaceCollectible = Controller.instance.southeastSpaceCollectible;
        SpaceCollectibleHandler southSpaceCollectible = Controller.instance.southSpaceCollectible;
        SpaceCollectibleHandler southwestSpaceCollectible = Controller.instance.southwestSpaceCollectible;
        SpaceCollectibleHandler westSpaceCollectible = Controller.instance.westSpaceCollectible;
        SpaceCollectibleHandler northwestSpaceCollectible = Controller.instance.northwestSpaceCollectible;

        northSCSaveData.LoadIntoCollectible(northSpaceCollectible);
        northeastSCSaveData.LoadIntoCollectible(northeastSpaceCollectible);
        eastSCSaveData.LoadIntoCollectible(eastSpaceCollectible);
        southeastSCSaveData.LoadIntoCollectible(southeastSpaceCollectible);
        southSCSaveData.LoadIntoCollectible(southSpaceCollectible);
        southwestSCSaveData.LoadIntoCollectible(southwestSpaceCollectible);
        westSCSaveData.LoadIntoCollectible(westSpaceCollectible);
        northwestSCSaveData.LoadIntoCollectible(northwestSpaceCollectible);

        BridgeOfcSlot bridgeOfcSlot_0 = bridgeOfcManager.bridgeOfcSlot_0;
        BridgeOfcSlot bridgeOfcSlot_1 = bridgeOfcManager.bridgeOfcSlot_1;
        BridgeOfcSlot bridgeOfcSlot_2 = bridgeOfcManager.bridgeOfcSlot_2;
        BridgeOfcSlot bridgeOfcSlot_3 = bridgeOfcManager.bridgeOfcSlot_3;

        BOfc_0_SaveData.LoadIntoOfcSlot(bridgeOfcSlot_0);
        BOfc_1_SaveData.LoadIntoOfcSlot(bridgeOfcSlot_1);
        BOfc_2_SaveData.LoadIntoOfcSlot(bridgeOfcSlot_2);
        BOfc_3_SaveData.LoadIntoOfcSlot(bridgeOfcSlot_3);
    }
}
