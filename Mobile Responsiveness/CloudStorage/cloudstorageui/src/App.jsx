import { useState, useEffect, useRef, useCallback } from "react";
import { BlockBlobClient } from "@azure/storage-blob";
import { Button } from "react-bootstrap";

import "./App.css";

const App = () => {
  const portNumber = 44303; /* visual studio port number here */
  const mainUrl = "https://localhost:" + portNumber + "/api/images";
  // State management
  const [apiVersion, setAPIVersion] = useState("?api-version=1.0");
  const [activeButton, setActiveButton] = useState("one");
  const [error, setError] = useState();
  const [name, setName] = useState("");
  const [description, setDescription] = useState("");
  const [images, setImages] = useState([]);
  const [uploadProgress, setUploadProgress] = useState(0);
  const [imageSize, setImageSize] = useState(200); // Default 200px
  const [isLoading, setisLoading] = useState(false); //Default loading to false
  const [toastMessage, setToastMessage] = useState("");
  const inputFileRef = useRef();

  // Validate port number on mount
  useEffect(() => {
    if (portNumber <= 0) {
      setError("You need to set your port number in App.js.");
    }
  }, [portNumber]);

  // Handle image upload to Azure Blob Storage
  const onClickUpload = useCallback(() => {
    setUploadProgress(0);
    setisLoading(true);
    setToastMessage("Creating image...");
    setError("");

    // Validate input
    if (!name || name.trim().length < 3) {
      setError("Enter at least three characters for the title.");
      setToastMessage("Error!");
      setisLoading(false)
      return;
    }

    if(apiVersion === "?api-version=1.1") {
      if (!description || description.trim().length < 5) {
      setError("Enter at least five characters for the description.");
      setToastMessage("Error!");
      setisLoading(false)
      return;
    }
    }
   
    var file = inputFileRef.current.files[0];
    if (!file) {
      setError("Choose a file.");
      setToastMessage("Error!");
      setisLoading(false)
      return;
    }

    const body = {
      Name: name
    };
    
    if(apiVersion === "?api-version=1.1") {
      body.Description = description;
    };

    //Post
    // Create new image record in database
    fetch(mainUrl + `${apiVersion}`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(body),
    })
      .then((response) => response.json())
      .then((createdImageDetails) => {
        setToastMessage("Uploading file to Azure...");
        // Upload file to Azure Blob Storage with progress tracking
        const file = inputFileRef.current.files[0];
        const blockBlobClient = new BlockBlobClient(
          createdImageDetails.uploadUrl
        );
        return blockBlobClient.uploadData(file, {
          blockSize: 1024 * 64, // 64kb blocks
          concurrency: 1,
          onProgress:(progressEvent) => {
        console.log("Progress:", progressEvent.loadedBytes, "/", file.size);
        const percentComplete = Math.round((progressEvent.loadedBytes / file.size) * 100)
        setUploadProgress(percentComplete);
        }}).then(() => createdImageDetails);
      })
      .then((createdImageDetails) => {
        // Notify server that upload is complete
      setToastMessage("Upload Complete!");
      const putURL = `${mainUrl}/${createdImageDetails.id}/uploadComplete${apiVersion}`;
        return fetch(putURL, {
      method: "PUT",
      headers: {
        "Content-Type": "application/json",
      },
      })
      })
      .then((uploadCompleteResult) => {
        return uploadCompleteResult.json();
      })
      .then((uploadCompleteJson) => {
        // Add new image to display list
        setImages((prevImages) => [...prevImages, uploadCompleteJson])
      }).finally(() => {
        setisLoading(false);
      });
  }, [mainUrl, name, description, apiVersion]);

  const swapAPIVersion = (() => {
    setToastMessage("Swapping API Version...");
    //Is api version currently 1.0?
    var currentAPIVersion = apiVersion 
    if (currentAPIVersion === "?api-version=1.0") {
    currentAPIVersion = "?api-version=1.1"; //Yes
    setActiveButton("two")
    } else {
    currentAPIVersion = "?api-version=1.0"; //No
    setActiveButton("one")
    }
    setAPIVersion(currentAPIVersion);
  });

  // Load existing images on mount
  useEffect(() => {
    setisLoading(true);
    setToastMessage("Loading existing images...");
    if (!mainUrl || portNumber <= 0) {
      return;
    }

    fetch(mainUrl + `${apiVersion}`)
      .then((response) => {
        if (!response.ok) {
          throw new Error("Not OK status code: " + response.status);
        }
        return response.json();
      })
      .then((responseJson) => {
        setImages(responseJson);
        setToastMessage("Done!");
      }).finally(() => {
        setisLoading(false);
      })
      .catch((error) => {
        setisLoading(false);
        setToastMessage("Error!");
        setError("Failed to load images on start: " + error);
      });
  }, [mainUrl, apiVersion]);

  // Delete all images
  const onClickPurge = useCallback(() => {
    setToastMessage("Purging all images...")
    setisLoading(true);
    fetch(mainUrl + apiVersion, {
      method: "DELETE",
    }).then((result) => {
      if (result.ok) {
        setImages([]);
      }
      setToastMessage("Done!")
    }).finally(() => {
        setisLoading(false);
      });
  }, [mainUrl, apiVersion]);

  return (
    <div className="App">
      {/* Name input */}
      <div className="controlsContainer">
        Name:{" "}
        <input
          disabled={isLoading === true}
          value={name}
          onChange={(e) => setName(e.target.value)}
          type="text"
        />
      </div>

      {/* Description input */}
      <div className="controlsContainer">
        Description:
        <input
          value={description}
          disabled={apiVersion === "?api-version=1.0" || isLoading === true}
          onChange={(e) => setDescription(e.target.value)}
          type="text"
        />
      </div>

       {/* File picker */}
      <div className="controlsContainer">
        <input disabled={isLoading === true} ref={inputFileRef} type="file" accept="image/*" className="water-button"/>
      </div>

      {/* Action buttons */}
      <div className="controlsContainer">
        <Button disabled={isLoading === true} onClick={onClickUpload} className="air-button">Upload</Button>
        <Button disabled={isLoading === true} onClick={onClickPurge} className="fire-button">Purge Images</Button>
        <Button disabled={activeButton === "one" || isLoading === true} onClick={swapAPIVersion} className="water-button">API Version 1.0</Button>
        <Button disabled={activeButton === "two" || isLoading === true} onClick={swapAPIVersion} className="water-button">API Version 1.1</Button>
      </div>

      {/* Image size slider */}
      <div className="controlsContainer">
        <label htmlFor="imageSizeSlider">Image Size: {imageSize}px</label>
        <input
          disabled={isLoading === true}
          id="imageSizeSlider"
          type="range"
          min="50"
          max="500"
          value={imageSize}
          onChange={(e) => setImageSize(Number(e.target.value))}
          className="image-slider"/>
      </div>

      {/* Error display */}
      {error && <div className="error">{error}</div>}

      {/* Upload progress bar */}
      {uploadProgress > 0 && uploadProgress < 100 && (
        <div className="progressContainer">
          <div className="progressBar" style={{ width: `${uploadProgress}%` }}>
            {uploadProgress}%
          </div>
        </div>
      )}
      
      {/* Image gallery */}
      <div className="imagesContainer">
        {images.map((img) => {
          var srcGetURL = `${mainUrl}/${img.id}${apiVersion}`;
          return <img key={img.id} src={srcGetURL} alt={img.name} style={{ width: `${imageSize}px`, height: 'auto' }}></img>;
         })}
      </div>

      {/* Toast Message Popup (only when loading) */}
      {isLoading && (
      <div className="toastMessage">
      {toastMessage}
      </div>
      )}

    </div>
  );
};

export default App;