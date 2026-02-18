import react from "@vitejs/plugin-react";
import { defineConfig } from "vite";

// https://vitejs.dev/config/
export default defineConfig({
  plugins: [react()],
  server: {
    open: "http://localhost:3000",
    port: 3000,
    strictPort: false,
  },
});
