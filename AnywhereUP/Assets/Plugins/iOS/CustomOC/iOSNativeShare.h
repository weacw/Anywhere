//
//  SocialShare.h
//  UnityBridgeShare
//
//  Created by NSWell on 2018/4/8.
//  Copyright © 2018年 NSWell. All rights reserved.
//

#import "UnityAppController.h"

@interface iOSNativeShare : UIViewController
{
    UINavigationController *navController;
}


struct ConfigStruct {
    char* title;
    char* message;
};

struct SocialSharingStruct {
    char* text;
    char* subject;
    char* filePaths;
};


#ifdef __cplusplus
extern "C" {
#endif
    
    void showAlertMessage(struct ConfigStruct *confStruct);
    void showSocialSharing(struct SocialSharingStruct *confStruct);
    
#ifdef __cplusplus
}
#endif


@end
