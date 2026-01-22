import type { NextConfig } from "next";

const nextConfig: NextConfig = {
  experimental: {
    // @ts-expect-error - Turbopack root config
    turbopack: {
      root: process.cwd()
    }
  }
};

export default nextConfig;
