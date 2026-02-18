import { useState, useEffect, useRef, useCallback } from "react";
import { BlockBlobClient } from "@azure/storage-blob";
import { Button } from "react-bootstrap";

import "./App.css";

const App = () => {
  const portNumber = 44303; /* put your visual studio port number here */
  const mainUrl = "https://localhost:" + portNumber + "/api/images";

  // State management
  const [error, setError] = useState();
  const [name, setName] = useState("");
  const [images, setImages] = useState([]);
  const [uploadProgress, setUploadProgress] = useState(0);
  const [imageSize, setImageSize] = useState(200); // Default 200px
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
    setError("");

    // Validate input
    if (!name || name.length < 3) {
      setError("Enter at least three characters for the title.");
      return;
    }

    var file = inputFileRef.current.files[0];
    if (!file) {
      setError("Choose a file.");
      return;
    }

    // Create new image record in database
    fetch(mainUrl, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify({
        Name: name,
      }),
    })
      .then((response) => response.json())
      .then((createdImageDetails) => {
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
        const putURL = `${mainUrl}/${createdImageDetails.id}/uploadComplete`;
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
      });
  }, [mainUrl, name]);

  // Load existing images on mount
  useEffect(() => {
    if (!mainUrl || portNumber <= 0) {
      return;
    }

    fetch(mainUrl)
      .then((response) => {
        if (!response.ok) {
          throw new Error("Not OK status code: " + response.status);
        }
        return response.json();
      })
      .then((responseJson) => {
        setImages(responseJson);
      })
      .catch((error) => {
        setError("Failed to load images on start: " + error);
      });
  }, [mainUrl]);

  // Delete all images
  const onClickPurge = useCallback(() => {
    fetch(mainUrl, {
      method: "DELETE",
    }).then((result) => {
      if (result.ok) {
        setImages([]);
      }
    });
  }, [mainUrl]);

  return (
    <div className="App">
      {/* Name input and file picker */}
      <div className="controlsContainer">
        Name:{" "}
        <input
          value={name}
          onChange={(e) => setName(e.target.value)}
          type="text"
        />
        <input ref={inputFileRef} type="file" accept="image/*" className="water-button"/>
      </div>

      {/* Action buttons */}
      <div className="controlsContainer">
        <Button onClick={onClickUpload} className="air-button">Upload</Button>
        <Button onClick={onClickPurge} className="fire-button">Purge Images</Button>
      </div>

      {/* Image size slider */}
      <div className="controlsContainer">
        <label htmlFor="imageSizeSlider">Image Size: {imageSize}px</label>
        <input
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
          var srcGetURL = `${mainUrl}/${img.id}`;
          return <img key={img.id} src={srcGetURL} alt={img.name} style={{ width: `${imageSize}px`, height: 'auto' }}></img>;
         })}
      </div>
    </div>
  );
};

export default App;