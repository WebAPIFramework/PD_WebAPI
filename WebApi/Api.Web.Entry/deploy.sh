remote_server=192.168.10.95
# �ϴ�
echo "start upload"
scp -r D:/Release/furionPublish/* root@$remote_server:/home/docker/proadmin_api/Api
echo "upload successfully"

# ִ��Զ�̾��񹹽���������ͣ
echo "exec remote script for image build and container restart"
ssh root@$remote_server "cd /home/docker/proadmin_api ; sh build.sh"
echo "restart successfully"
