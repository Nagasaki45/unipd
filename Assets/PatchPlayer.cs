using LibPDBinding;
using UnityEngine;


public class PatchPlayer : MonoBehaviour
{
    private int patchHandle;
    private int numberOfTicks;


    void Start()
    {
        initPd();
        patchHandle = LibPD.OpenPatch(Application.streamingAssetsPath + "/randomOsc.pd");
        LibPD.ComputeAudio(true);
    }


    // Unity audio callback
    public void OnAudioFilterRead(float[] data, int channels)
    {
        LibPD.Process(numberOfTicks, data, data);
    }


    // Close patch and release Pd on quit
    void OnDestroy()
    {
        LibPD.ClosePatch(patchHandle);
        LibPD.Release();
    }


    // Initialise Pd with Unity's sample rate and calculate number of ticks. Returns 0 on success.
    public int initPd()
    {
        int bufferSize;
        int noOfBuffers;
        AudioSettings.GetDSPBufferSize(out bufferSize, out noOfBuffers);

        // Calculate number of ticks for process callback
        numberOfTicks = bufferSize / LibPD.BlockSize;

        // Get Unity's sample rate
        int unitySR = AudioSettings.outputSampleRate;

        // Initialise Pd with 2 ins and outs and Unity's samplerate. Project dependent.
        return LibPD.OpenAudio(2, 2, unitySR);
    }
}
