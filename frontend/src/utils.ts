export const decodeToken = (token: string | null): {Username: string; sub: string; exp: number } | null => {
    try {
      if (!token) {
        return null;
      }

      const base64Url = token.split(".")[1]; // Extract payload (the second part of the JWT)
      const base64 = base64Url.replace(/-/g, "+").replace(/_/g, "/"); // Convert URL-safe base64 to standard base64
      const jsonPayload = atob(base64); // Decode base64 to get the JSON payload
  
      return JSON.parse(jsonPayload); 
    } catch (error) {
      console.error("Invalid token:", error);
      return null; 
    }
  };