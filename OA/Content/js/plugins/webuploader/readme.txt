
///
///webuploaderext ����˵��
///by:willian date:2016-10-27
///

������http://fex.baidu.com/webuploader/getting-started.htm
js���� 
/dist/webuploader.min.js
/dist/webuploaderext.min.js

��������
$("#myUploader").powerWebUpload({
            auto: true,  fileNumLimit: 5, formData: {
                uptype: "product",//�ϴ�����
                token: "token"//��֤���� ���ܴ�����
            },
            accept: {
                title: 'Images',
                extensions: 'gif,jpg,jpeg,bmp,png', //�ļ�����
               // mimeTypes: 'image/*'
            },
            fileSingleSizeLimit: 5,//�����ļ���С���ƣ���λM��Ĭ��5��
  		fileNumLimit: 1,//�ļ��ܸ������ƣ�Ĭ��Ϊ1��
        });

requirejs ����


define('upload',['jquery','webuploader'],function($,WebUploader){
     
     window['WebUploader']=WebUploader ;
   require(['webuploaderext'],function(){
	   
	    $("#myUploader").powerWebUpload({
            auto: true, fileNumLimit: 5, formData: {
               uptype: "product",//�ϴ�����
                token: "token"//��֤���� ���ܴ�����
            },
            accept: {
                title: 'Images',
                extensions: 'gif,jpg,jpeg,bmp,png',//�ļ�����
                // mimeTypes: 'image/*'
            },  
            fileSingleSizeLimit: 5,//�����ļ���С���ƣ���λM��Ĭ��5��
  		fileNumLimit: 1,//�ļ��ܸ������ƣ�Ĭ��Ϊ1��
        });
   });
��ע��
�ϴ��ɹ����������name���Ʒֱ�Ϊ 
fileUrl url��ַ
fileName �����ϴ��ļ���

��̨��ȡ������ʱ���ر�ע��

