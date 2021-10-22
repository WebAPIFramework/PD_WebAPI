remote_server=192.168.10.95
# 上传
echo "start upload"
scp -r D:/Release/furionPublish/* root@$remote_server:/home/docker/proadmin_api/Api
echo "upload successfully"

# 执行远程镜像构建与容器启停
echo "exec remote script for image build and container restart"
ssh root@$remote_server "cd /home/docker/proadmin_api ; sh build.sh"
echo "restart successfully"
