import type { NextConfig } from "next";

const nextConfig: NextConfig = {
  experimental: {
    // @ts-expect-error - Turbopack root/memory config is distinct in newer versions but valid
    turbopack: {
      root: process.cwd()
    }
  }
};

export default nextConfig;
