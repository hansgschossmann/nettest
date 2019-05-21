# nettest tool
Tool to display network interface information and test connectivity to a user provided URL.

## Features
- shows all interfaces on the container
- shows gateway setting
- shows DNS server setting
- allows to enter an URL to ping
- provides a responder path, which returns the info listed above

## Getting Started

Start with "docker run -p 8080:80 hansgschossmann/nettest" and connect by browsing "localhost:8080"

### Prerequisites

The implementation is based on .NET Core so it is cross-platform and recommended hosting environment is docker.

### Installation

There is no installation required.

### Quickstart

A docker container of the component is hosted on dockerhub and can be pulled by:

docker pull hansgschossmann/nettest

