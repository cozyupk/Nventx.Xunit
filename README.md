# Shadow DI Playground

This repository contains an early-stage prototype of a lightweight dependency injection mechanism, inspired by the idea of *Shadow DI*.  
The core concept is: **declare your contracts, drop your implementations, and let the magic happen—without physical references**.

> ⚠️ This is a work in progress. Expect sharp edges, missing docs, and some TODO comments whispering to the void.

## What’s This?

Shadow DI is a reflection-based, attribute-driven mechanism that:
- Automatically discovers and registers implementations from external assemblies
- Avoids static references between contract and implementation layers
- Embraces the spirit of Open/Closed Principle with minimal friction

## Status

Currently under development and **not production-ready**.  
If you’re curious, feel free to explore or contribute, but don’t expect stability (yet!).

## Getting Started

This README will be updated soon with setup instructions and examples.  
In the meantime, you can take a look at the [`EntryPointWithShadowDI`](./src/...) project for a rough idea of how things work.

## License

MIT License.  
Use it, break it, fork it, improve it—just don’t sue me if your app explodes.

---

*This project may eventually grow into a more robust framework, or it may just be an experiment that lives happily in the shadows.* ☁️
