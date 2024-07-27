FROM mcr.microsoft.com/devcontainers/dotnet:1-8.0-bookworm
USER vscode
RUN git clone --depth 1 https://github.com/junegunn/fzf.git $HOME/.fzf && \
    echo "y" "y" "y" | $HOME/.fzf/install && \
    echo "eval $(fzf --bash)" >> $HOME/.bashrc
RUN DEBIAN_FRONTEND=noninteractive sudo apt update && sudo apt upgrade -y && \
    sudo apt install autojump -y --no-install-recommends && \
    echo ". /usr/share/autojump/autojump.sh" >> ~/.bashrc